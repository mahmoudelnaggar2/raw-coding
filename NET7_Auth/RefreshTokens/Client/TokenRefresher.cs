namespace Client;

public class TokenRefresher : BackgroundService
{
    private readonly ILogger<TokenRefresher> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TokenRefresher(
        ILogger<TokenRefresher> logger,
        IServiceProvider serviceProvider
        )
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken
    )
    {
        while (true)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TokenDatabase>();
                var refreshTokenContext = scope.ServiceProvider.GetRequiredService<RefreshTokenContext>();
                var tokens = db.Record;
                foreach (var (patreonId, tokenInfo) in tokens)
                {
                    if (tokenInfo.Expires.Subtract(DateTime.UtcNow) < TimeSpan.FromDays(1))
                    {
                        _logger.LogInformation($"refreshing token for {patreonId}");
                        var result = await refreshTokenContext.RefreshTokenAsync(tokenInfo, stoppingToken);
                        db.Save(patreonId, new TokenInfo(
                            result.AccessToken,
                            result.RefreshToken,
                            DateTime.UtcNow.AddSeconds(int.Parse(result.ExpiresIn))
                        ));
                    }
                }
            }
            catch
            {
                
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}