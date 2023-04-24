namespace Heaps;

public class ForceGen
{
    public static string HandleStart(List<Longer[]> bin)
    {
        Console.WriteLine("/start");
        
        var collection = Enumerable.Range(0, 10000)
            .Select(i => new Longer() { Number = i, Worder = i.ToString() })
            .ToArray();
        
        bin.Add(collection);
        return "ok";
    }

    public static string HandlePromotion(List<Longer[]> bin)
    {
        Console.WriteLine("/promote");
        Console.WriteLine("Generation Collection: 0");
        Console.WriteLine($"bin:{GC.GetGeneration(bin)}");
        Console.WriteLine($"0:{GC.GetGeneration(bin[0])}");
        GC.Collect(0);
        Console.WriteLine($"bin:{GC.GetGeneration(bin)}");
        Console.WriteLine($"0:{GC.GetGeneration(bin[0])}");
        bin.RemoveAt(0);
        return "ok";
    }

    public static string HandleSweep(List<Longer[]> bin)
    {
        Console.WriteLine("/sweep");
        Console.WriteLine("Sweeping Generation: 1");
        Console.WriteLine($"bin:{GC.GetGeneration(bin)}");
        GC.Collect(1);
        Console.WriteLine($"bin:{GC.GetGeneration(bin)}");
        return "ok";
    }

    public static string HandleCompact(List<Longer[]> bin)
    {
        Console.WriteLine("/compact");
        Console.WriteLine($"Compacting Generation: 1");
        Console.WriteLine($"bin:{GC.GetGeneration(bin)}");
        GC.Collect(1, GCCollectionMode.Default, true, compacting: true);
        Console.WriteLine($"bin:{GC.GetGeneration(bin)}");
        return "ok";
    }
}

public class Longer
{
    public int Number { get; set; }
    public string Worder { get; set; }

    ~Longer()
    {
        Console.WriteLine("bye bye");
    }
}