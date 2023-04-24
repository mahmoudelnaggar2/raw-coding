using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DefinedBoundaries;

public class Database : DbContext
{
    public DbSet<ProductDescription> Products { get; set; }
    public DbSet<ProductStock> Stock { get; set; }
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<CatalogItemOption> ItemOptions { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public Database(DbContextOptions<Database> options)
        : base(options)
    {
    }

    public static async Task InitDatabase(IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Database>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();

        var product = new ProductDescription()
        {
            Name = "Shirt",
            Description = "buy my t-shirt",
            Stock = new List<ProductStock>()
            {
                new ProductStock()
                {
                    Description = "Small",
                    Value = 1000,
                    Qty = 100
                }
            }
        };
        db.Products.Add(product);
        await db.SaveChangesAsync();

        db.CatalogItems.Add(new CatalogItem()
        {
            Name = "Shirt",
            Description = "buy my t-shirt",
            ProductId = product.Id,
            Options = new List<CatalogItemOption>()
            {
                new CatalogItemOption()
                {
                    Description = "Small",
                    Value = 1000,
                }
            }
        });
        
        db.Carts.Add(new Cart()
        {
            Items = new List<CartItem>()
            {
                new CartItem()
                {
                    ProductName = "Short",
                    ProductDescription = "buy my t-shirt",
                    OptionDescription = "Small",
                    Value = 1000,
                    Qty = 2,
                    ProductId = product.Id,
                }
            }
        });

        await db.SaveChangesAsync();
        db.Orders.Add(new Order()
        {
            Items = new List<OrderItem>()
            {
                new OrderItem()
                {
                    ProductName = "Short",
                    ProductDescription = "buy my t-shirt",
                    OptionDescription = "Small",
                    Value = 1000,
                    Qty = 1,
                    ProductId = product.Id,
                }
            }
        });

        await db.SaveChangesAsync();
    }
}