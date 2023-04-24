namespace Heaps;

public class ObjectAllocation
{
    public static string Handle()
    {
        int tippingPoint = (85000 / sizeof(long)) - 3;
        
        Console.WriteLine($"{tippingPoint} bytes");
        
        var listSoh = Enumerable.Range(0, tippingPoint - 1)
            .Select(i => new ComplexObject() { Id = i, Name = i.ToString()})
            .ToArray();
        
        Console.WriteLine($"listSoh in gen:{GC.GetGeneration(listSoh)}");
        
        var listLoh = Enumerable.Range(0, tippingPoint)
            .Select(i => new ComplexObject() { Id = i, Name = i.ToString() })
            .ToArray();
        
        Console.WriteLine($"listLoh in gen:{GC.GetGeneration(listLoh)}");
        
        return "ok";
    }
}

public class ComplexObject
{
    public int Id { get; set; }
    public string Name { get; set; }
}