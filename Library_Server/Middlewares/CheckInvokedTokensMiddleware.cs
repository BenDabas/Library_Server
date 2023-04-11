using Library_Server.DB;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Library_Server.Middlewares
{
    public class CheckInvokedTokensMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public CheckInvokedTokensMiddleware(ILogger<CheckInvokedTokensMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ApplicationDbContext applicationDbContext)
        {
            _logger.LogInformation("Start: CheckInvokedTokensMiddleware");
            var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var tokenStr = authorizationHeader.Substring("Bearer ".Length).Trim();
                var invokedToken = await applicationDbContext.RevokedTokens.SingleOrDefaultAsync(t => t.Token == tokenStr);

                if (invokedToken != null)
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Invalid token.");
                    return;
                }
            }

            await _next(httpContext);

            _logger.LogInformation("End: CheckInvokedTokensMiddleware");

        }

    }
}
