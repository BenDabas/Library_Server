using Library_Server.Dtos.User;
using Library_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Server.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var authenticatedUser = _authService.AuthenticateUser(loginRequestDto.Username, loginRequestDto.Password);

            if (authenticatedUser == null)
                return BadRequest("Username or password is incorrect.");

            var token = _authService.GenerateToken(authenticatedUser);

            return Ok(new { Token = token });
        }
    }
}
