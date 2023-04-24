using Microsoft.EntityFrameworkCore;
using DefinedBoundaries;

var builder = WebApplication.CreateBuilder(args);

MessageProcessor.Register(builder.Services);
builder.Services.AddDbContext<Database>(o =>
    o.UseSqlite($"Data Source={Path.Join(builder.Environment.ContentRootPath, "app.db")}"));

var app = builder.Build();

await Database.InitDatabase(app.Services);

app.MapGet("/products", (Database db) => db.Products.Include(x => x.Stock).ToListAsync());

app.MapGet("/catalog", (Database db) => db.CatalogItems.Include(x => x.Options).ToListAsync());

app.MapGet("/cart", (Database db) => db.Carts.Include(x => x.Items).ToListAsync());

app.MapGet("/orders", (Database db) => db.Orders.Include(x => x.Items).ToListAsync());

app.MapGet("/update-product", async (Database db, IMessageSink sink) =>
    {
        var newPrice = Random.Shared.Next();
        await db.Stock.ExecuteUpdateAsync(s => s.SetProperty(m => m.Value, newPrice));
        await sink.Consume(new ProductPublished() { ProductId = 1 });
    }
);

app.Run();