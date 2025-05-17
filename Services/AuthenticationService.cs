using AutoMapper;
using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ServicesAbstraction;
using Shared.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager,
                                       IOptions<JwtOptions> _jwtOptions,
                                       IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AddressDto> GetUserAddressAsync(string email)
        {
            //map from address dto to address
            var user = await _userManager.Users
                                         .Include(u => u.Address)
                                         .FirstOrDefaultAsync(u => u.Email == email)
                                         ?? throw new UserNotFoundException(email);

            if (user.Address is not null)
                return _mapper.Map<AddressDto>(user.Address);
            throw new AddressNotFoundException(user.UserName!);
        }

        public async Task<UserResponse> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
                return new UserResponse()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Token = await GenerateToken(user)
                };
            throw new UserNotFoundException(email);
        }

        public async Task<UserResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email) ??
                       throw new UserNotFoundException(loginRequest.Email);
            var isValidPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (isValidPassword)
                return new UserResponse
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Token = await GenerateToken(user)
                };
            throw new UnauthorizedException();
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = new ApplicationUser()
            {
                Email = registerRequest.Email,
                UserName = registerRequest.Username,
                DisplayName = registerRequest.DisplayName,
                PhoneNumber = registerRequest.PhoneNumber,
            };
            var createUser = await _userManager.CreateAsync(user, registerRequest.Password);
            if (createUser.Succeeded)
                return new UserResponse()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await GenerateToken(user)
                };
            var errors = createUser.Errors.Select(e => e.Description).ToList();
            throw new BadRequestException(errors);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto, string email)
        {
            var user = await _userManager.Users
                                         .Include(u => u.Address)
                                         .FirstOrDefaultAsync(u => u.Email == email)
                                         ?? throw new UserNotFoundException(email);
            if (user.Address is not null)
            {
                user.Address.FisrtName = addressDto.FisrtName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Country = addressDto.Country;
                user.Address.City = addressDto.City;
                user.Address.Street = addressDto.Street;
            }
            else
            {
                user.Address = _mapper.Map<Address>(addressDto);
                user.Address.Id = Guid.NewGuid().ToString();
            }
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtOpts = _jwtOptions.Value;
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            string secretKey = jwtOpts.SecretKey;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOpts.Issuer,
                audience: jwtOpts.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwtOpts.DurationInDays),
                signingCredentials: credentials
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
