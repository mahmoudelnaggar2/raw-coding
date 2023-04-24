using Microsoft.EntityFrameworkCore;
using Spagetti;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Database>(o =>
    o.UseSqlite($"Data Source={Path.Join(builder.Environment.ContentRootPath, "app.db")}"));

var app = builder.Build();

await Database.InitDatabase(app.Services);

app.MapGet("/products", (Database db) => db.Products
    .Select(x => new
    {
        Type = "product",
        x.Name,
        x.Description,
        Stock = x.Stock.Where(x => !x.Disabled).Select(s => new { s.Description, s.Qty, s.Value })
    })
    .ToListAsync()
);

app.MapGet("/cart", (Database db) => db.Carts
    .Select(x => new
    {
        x.Id,
        Type = "cart",
        Products = x.Products.AsQueryable().Select(y => new
        {
            y.ProductStock.Description,
            y.ProductStock.Qty,
            y.ProductStock.Value,
            ProductName = y.ProductStock.ProductDescription.Name,
            ProductDescription = y.ProductStock.ProductDescription.Description,
        }).ToList()
    })
    .ToListAsync()
);

app.MapGet("/orders", (Database db) => db.Orders
    .Select(x => new
    {
        x.Id,
        Type = "order",
        Products = x.Products.AsQueryable().Select(y => new
        {
            y.ProductStock.Description,
            y.ProductStock.Qty,
            y.ProductStock.Value,
            ProductName = y.ProductStock.ProductDescription.Name,
            ProductDescription = y.ProductStock.ProductDescription.Description,
        }).ToList()
    })
    .ToListAsync()
);

app.MapGet("/update-product", async (Database db) =>
    {
        var newPrice = Random.Shared.Next();
        await db.Stock.ExecuteUpdateAsync(s => s.SetProperty(m => m.Value, newPrice));
    }
);

app.MapGet("/update-product-safe", async (Database db) =>
    {
        var newPrice = Random.Shared.Next();
        var stock = await db.Stock.FirstAsync();
        stock.Disabled = true;
        
        db.Stock.Add(
            new ProductStock()
            {
                ProductDescriptionId = stock.ProductDescriptionId,
                Description = stock.Description,
                Value = newPrice,
                Qty = stock.Value
            }
        );

        await db.SaveChangesAsync();
        await db.CartProducts.Where(x => x.ProductStockId == stock.Id).ExecuteDeleteAsync();
    }
);

app.Run();