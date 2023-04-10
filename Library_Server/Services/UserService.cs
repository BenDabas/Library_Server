using Library_Server.DB;
using Library_Server.Dtos.User;
using Library_Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Library_Server.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authenticationService;

        public UserService(ApplicationDbContext context, AuthService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<User> AddUser(RegisterRequestDto registerRequestDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == registerRequestDto.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var user = new User(registerRequestDto.Username,
                registerRequestDto.Name,
                registerRequestDto.Email,
                registerRequestDto.Age,
                _authenticationService.HashPassword(registerRequestDto.Password));

            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
