using Library_Server.Dtos.User;
using Library_Server.Models;
using Library_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger _logger;

        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            try
            {
                var user = await _userService.AddUser(registerRequestDto);
                return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
