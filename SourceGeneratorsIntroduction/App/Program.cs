// See https://aka.ms/new-console-template for more information

using App;

var car = new Car();
car.Do();

new Tiger().Yo();

var p = new Person() { FirstName = "Foo", LastName = "Bar" };
Console.WriteLine(p.FullName());

public partial class Car
{
    [Give("Print")]
    public partial void Do();
}


public partial class Tiger
{
    [Give("Print")]
    public partial void Yo();
}

public partial class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Give("FullName")]
    public partial string FullName();
}

public class Functions
{
    [Define]
    public static void Print()
    {
        Console.WriteLine("Hello World");
    }

    [Define]
    public string FullName()
    {
        var type = GetType();
        var firstNameP = type.GetProperty("FirstName");
        var lastNameP = type.GetProperty("LastName");

        var fn = (string)firstNameP.GetValue(this);
        var ln = (string)lastNameP.GetValue(this);

        return $"{fn} {ln}";
    }
}