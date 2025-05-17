using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController(IServiceManager _serviceManager) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
        {
            return Ok(await _serviceManager.AuthenticationService.LoginAsync(loginRequest));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterRequest registerRequest)
        {
            return Ok(await _serviceManager.AuthenticationService.RegisterAsync(registerRequest));
        }

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetUser()
        {
            var mail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.AuthenticationService.GetUserByEmailAsync(mail));
        }

        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            return Ok(await _serviceManager.AuthenticationService.CheckEmailAsync(email));
        }

        [HttpGet("GetAddress")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            var mail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.AuthenticationService.GetUserAddressAsync(mail));
        }

        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
        {
            var mail = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.AuthenticationService.UpdateUserAddressAsync(addressDto, mail));
        }

    }
}
