namespace DefinedBoundaries;

#region Management

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
}

#endregion

#region Store

public class CatalogItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<CatalogItemOption> Options { get; set; }

    public int ProductId { get; set; }
}

public class CatalogItemOption
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
}

public class Cart
{
    public int Id { get; set; }

    public List<CartItem> Items { get; set; }
}

public class CartItem
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string OptionDescription { get; set; }
    public int Value { get; set; }
    public int Qty { get; set; }

    public int ProductId { get; set; }
}

#endregion

#region Customer

public class Order
{
    public int Id { get; set; }
    public List<OrderItem> Items { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string OptionDescription { get; set; }
    public int Value { get; set; }
    public int Qty { get; set; }

    public int ProductId { get; set; }
}

#endregion