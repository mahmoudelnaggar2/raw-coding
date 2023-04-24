using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;

namespace DefinedBoundaries;

public interface IMessages
{
}

public class ProductPublished : IMessages
{
    public int ProductId { get; set; }
}

public interface IMessageSink
{
    ValueTask Consume(IMessages msg);
}

public class MessageProcessor : BackgroundService, IMessageSink
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Channel<IMessages> _channel;

    public MessageProcessor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _channel = Channel.CreateUnbounded<IMessages>();
    }

    public ValueTask Consume(IMessages msg) => _channel.Writer.WriteAsync(msg);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var msg = await _channel.Reader.ReadAsync(stoppingToken);
                if (msg is ProductPublished pp) await ProcessProductPublished(pp);
            }
            catch
            {
            }
        }
    }

    private async Task ProcessProductPublished(ProductPublished msg)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Database>();

        // await db.CatalogItems.Where(x => x.ProductId == msg.ProductId).ExecuteDeleteAsync();
        var catalogItem = await db.CatalogItems.Include(x => x.Options).FirstAsync(x => x.ProductId == msg.ProductId);
        db.Remove(catalogItem);
        await db.SaveChangesAsync();
        
        await db.CartItems.Where(x => x.ProductId == msg.ProductId).ExecuteDeleteAsync();

        var product = await db.Products.Include(x => x.Stock).FirstAsync(x => x.Id == msg.ProductId);
        var newItem = new CatalogItem()
        {
            Name = product.Name,
            Description = product.Description,
            ProductId = product.Id,
            Options = product.Stock
                .Select(x => new CatalogItemOption()
                {
                    Description = x.Description,
                    Value = x.Value,
                })
                .ToList()
        };
        db.CatalogItems.Add(newItem);

        await db.SaveChangesAsync();
    }

    public static void Register(IServiceCollection services)
    {
        services.AddHostedService<MessageProcessor>();
        services.AddSingleton<IMessageSink>(sp => sp
            .GetRequiredService<IEnumerable<IHostedService>>()
            .OfType<MessageProcessor>()
            .First()
        );
    }
}