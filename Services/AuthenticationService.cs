using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
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
                                       IOptions<JwtOptions> _jwtOptions) : IAuthenticationService
    {
        public async Task<UserResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email) ??
                       throw new UserNotFoundException(loginRequest.Email);
            var isValidPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (isValidPassword)
                return new UserResponse
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
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
