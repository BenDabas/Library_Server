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
            try
            {
                var authenticatedUser = _authService.AuthenticateUser(loginRequestDto.Username, loginRequestDto.Password);

                if (authenticatedUser == null)
                    return BadRequest("Username or password is incorrect.");

                var token = _authService.GenerateToken(authenticatedUser);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return BadRequest("Invalid token.");
                }
                var tokenStr = authorizationHeader.Substring("Bearer ".Length).Trim();

                await _authService.AddRevokedToken(new LogoutRequestDto(tokenStr));
                return Ok("Logout success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
