// source: https://github.com/dotnet/aspnetcore/issues/14975

using System.Security.Claims;
using System.Threading.Channels;

public class AService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly ILogger<AService> _logger;

    public AService(
        IHttpContextAccessor accessor,
        ILogger<AService> logger
        )
    {
        _accessor = accessor;
        _logger = logger;
    }

    public async Task SomethingSlowAsync()
    {
        await Task.Delay(1500);
        // var x = _accessor.HttpContext.Items["x"];
        try
        {
            var x = _accessor.HttpContext.Items["x"];
        }
        catch (Exception e)
        {
            _logger.LogError(e, "oops");
        }
    }
}


public class BusinessLogic
{
    private readonly IHttpContextAccessor _accessor;
    private readonly ILogger<BusinessLogic> _logger;

    public BusinessLogic(
        IHttpContextAccessor accessor,
        ILogger<BusinessLogic> logger
        )
    {
        _accessor = accessor;
        _logger = logger;
    }

    public async Task NotifyUser()
    {
        try
        {
            var user = _accessor.HttpContext.User;

            var email = user.FindFirstValue("email");

            // notification logic
            _logger.LogInformation("notification successful!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed notification");
        }
    }
}


public class BackgroundNotifications : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly Channel<string> _channel;

    public BackgroundNotifications(IServiceProvider sp)
    {
        _sp = sp;
        _channel = Channel.CreateUnbounded<string>();
    }

    public ChannelWriter<string> Writer => _channel.Writer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var user_id = await _channel.Reader.ReadAsync(stoppingToken);
                using var scope = _sp.CreateScope();
                var bl = scope.ServiceProvider.GetRequiredService<BusinessLogic>();
                await bl.NotifyUser();
            }
            catch (Exception ignore)
            {
            }
        }
    }
}