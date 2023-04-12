using Library_Server.DB;
using Library_Server.Dtos.User;
using Library_Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Server.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(ApplicationDbContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        public User AuthenticateUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null || !VerifyPassword(password, user.HashPassword))
                return null;
            return user;
        }

        public async Task AddRevokedToken(LogoutRequestDto logoutRequestDto)
        {
            var jwtToken = new JwtSecurityToken(logoutRequestDto.Token);
            var revokedToken = new RevokedToken(logoutRequestDto.Token, jwtToken.ValidTo);

            _context.RevokedTokens.Add(revokedToken);
            await _context.SaveChangesAsync();
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(GetUserClaims(user)),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private IList<Claim> GetUserClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "User")
            };

            return claims;
        }

        public string HashPassword(string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return isPasswordCorrect;
        }
    }
}
