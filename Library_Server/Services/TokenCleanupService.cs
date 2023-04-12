using Library_Server.DB;

namespace Library_Server.Services
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenCleanupService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var expiredTokens = dbContext.RevokedTokens
                        .Where(t => t.ExpiryDate <= DateTime.UtcNow)
                        .ToList();

                    dbContext.RevokedTokens.RemoveRange(expiredTokens);
                    await dbContext.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
