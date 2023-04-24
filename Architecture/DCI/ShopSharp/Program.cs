// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Text;

public class Program
{
    public static void Main()
    {
        var shelf = new List<Product>() { new Product() { Name = "tomato", Value = 1000 } };
        var cart = new List<Product>() { new Product() { Name = "potato", Value = 5000 } };
        var pickProduct = new PickProduct(shelf, cart);
        pickProduct.Play();
        foreach (var p in cart)
        {
            Console.WriteLine("Product: " + p.Name + ", $" + p.Value);
        }
    }
}

public class PickProduct
{
    public PickProduct(
        List<Product> shelf,
        List<Product> cart
    )
    {
        Shelf = new ShelfRole(shelf);
        Cart = new CartRole(cart);
    }

    public ShelfRole Shelf { get; set; }
    public CartRole Cart { get; set; }
    
    public void Play()
    {
        var p = Shelf.Take();
        Cart.Place(p);
    }

    public record ShelfRole(List<Product> Products)
    {
        public Product Take()
        {
            var product = Products[0];
            Products.Remove(product);
            return product;
        }
    }

    public record CartRole(List<Product> Products)
    {
        public void Place(Product p)
        {
            Products.Add(p);
        }
    }
}

public struct Product
{
    public string Name { get; set; }
    public int Value { get; set; }
}