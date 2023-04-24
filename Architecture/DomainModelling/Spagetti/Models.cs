namespace Spagetti;

public class ProductDescription
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public List<ProductStock> Stock { get; set; }
}

public class ProductStock
{
    public int Id { get; set; }
    public int Qty { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
    public bool Disabled { get; set; }

    public int ProductDescriptionId { get; set; }
    public ProductDescription ProductDescription { get; set; }
}

public class Cart
{
    public int Id { get; set; }

    public List<CartProduct> Products { get; set; }
}

public class CartProduct
{
    public int CartId { get; set; }
    public Cart Cart { get; set; }

    public int ProductStockId { get; set; }
    public ProductStock ProductStock { get; set; }

    public int Qty { get; set; }
}

public class Order
{
    public int Id { get; set; }

    public List<OrderProduct> Products { get; set; }
}

public class OrderProduct
{
    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int ProductStockId { get; set; }
    public ProductStock ProductStock { get; set; }

    public int Qty { get; set; }
}