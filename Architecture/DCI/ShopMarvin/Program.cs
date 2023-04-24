using System;
using System.Collections.Generic;
using System.Text;

public class Program {

    public static void Main()
    {
        var shelf = new List<Product>() {new Product(){Name = "tomato", Value = 1000}};
        var cart = new List<Product>() {new Product(){Name = "potato", Value = 5000}};
        var pickProduct = new PickProduct(shelf, cart);
        pickProduct.Play();
        foreach(var p in cart)
        {
            Console.WriteLine("Product: " + p.Name + ", $" + p.Value);
        }
    }
}

public class PickProduct
{
    public PickProduct(List<Product> shelf, List<Product> cart)
    {
        Shelf = shelf;
        Cart = cart;
    }
    role Shelf
    {
        object Take()
        {
            var product = Shelf[0];
            ((List<Product>)Shelf).Remove(product);
            return product;
        }
    }

    role Cart
    {
        void Place(Product p)
        {
            Cart.Add(p);
        }
    }

    public void Play()
    {
        var p = Shelf.Take();
        Cart.Place(p);
    }

}

public struct Product
{
    public string Name {get;set;}
    public int Value {get;set;}
}