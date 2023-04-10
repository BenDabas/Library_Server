using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library_Server.Middlewares
{
    public class ExtractUserIdMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        private readonly string ClaimNameIdentifier = "nameid";
        public ExtractUserIdMiddleware(ILogger<ExtractUserIdMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Start: ExtractUserIdMiddleware");
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var tokenStr = authorizationHeader.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenStr);
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimNameIdentifier);

                if (userIdClaim != null)
                {
                    context.Items["UserId"] = userIdClaim.Value;
                }
            }
            _logger.LogInformation("End: ExtractUserIdMiddleware");
            await _next(context);
        }
    }
}
