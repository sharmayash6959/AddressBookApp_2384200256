using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using System.Threading.Tasks;

namespace AddressBook.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            var success = await _authService.RegisterUser(userDto);
            if (!success)
                return BadRequest("User already exists.");

            return Ok("User registered successfully.");
        }

        public IAuthService Get_authService()
        {
            return _authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDto, IAuthService _authService)
        {

            var token = await _authService.LoginUser(userDto.Email, userDto.Password);
            if (token == null)
                return Unauthorized("Invalid credentials.");

            return Ok(new { Token = token });
        }

        //

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO request)
        {
            bool result = await _authService.ForgotPasswordAsync(request);
            if (!result) return NotFound("User not found.");
            return Ok("Reset password link sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            bool result = await _authService.ResetPasswordAsync(request);
            if (!result) return BadRequest("Invalid or expired token.");
            return Ok("Password reset successfully.");
        }

        //
    }
}
