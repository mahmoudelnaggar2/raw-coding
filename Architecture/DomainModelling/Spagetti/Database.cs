using Microsoft.EntityFrameworkCore;

namespace Spagetti;

public class Database : DbContext
{
    public DbSet<ProductDescription> Products { get; set; }
    public DbSet<ProductStock> Stock { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartProduct> CartProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    public Database(DbContextOptions<Database> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartProduct>()
            .HasKey(x => new { x.CartId, x.ProductStockId });

        modelBuilder.Entity<CartProduct>()
            .HasOne(x => x.Cart)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CartId);

        modelBuilder.Entity<CartProduct>()
            .HasOne(x => x.ProductStock)
            .WithMany()
            .HasForeignKey(x => x.ProductStockId);

        modelBuilder.Entity<OrderProduct>()
            .HasKey(x => new { x.OrderId, x.ProductStockId });

        modelBuilder.Entity<OrderProduct>()
            .HasOne(x => x.Order)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(x => x.ProductStock)
            .WithMany()
            .HasForeignKey(x => x.ProductStockId);
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

        db.Carts.Add(new Cart()
        {
            Products = new List<CartProduct>()
            {
                new CartProduct() { ProductStock = product.Stock.First() }
            }
        });

        db.Orders.Add(new Order()
        {
            Products = new List<OrderProduct>()
            {
                new OrderProduct() { ProductStock = product.Stock.First() }
            }
        });

        await db.SaveChangesAsync();
    }
}