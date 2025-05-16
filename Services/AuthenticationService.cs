using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using ServicesAbstraction;
using Shared.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager) : IAuthenticationService
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
                    Token = "Token"
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
                    Token = "Token"
                };
            var errors = createUser.Errors.Select(e => e.Description).ToList();
            throw new BadRequestException(errors);
        }
    }
}
