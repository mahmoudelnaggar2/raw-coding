using Heaps;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<Longer[]>>();
builder.Services.AddSingleton<List<long[]>>();

var app = builder.Build();

app.MapGet("/", (List<long[]> bin) => bin);

app.MapGet("/fill", (int i, List<long[]> bin) =>
    {
        var collection = Enumerable.Range(0, i).Select(_ => Random.Shared.NextInt64()).ToArray();
        Console.WriteLine($"allocating {i * sizeof(long)} bytes");
        bin.Add(collection);
        
        Console.WriteLine($"bins:{GC.GetGeneration(bin)}");
        Console.WriteLine($"longs:{GC.GetGeneration(collection)}");
    }
);

app.MapGet("/clear", (List<long[]> bin) =>
    bin.Clear()
);

app.MapGet("start", ForceGen.HandleStart);
app.MapGet("promote", ForceGen.HandlePromotion);
app.MapGet("sweep", ForceGen.HandleSweep);
app.MapGet("compact", ForceGen.HandleCompact);

app.MapGet("obj", ObjectAllocation.Handle);
app.MapGet("list", InternalListArrayAllocation.Handle);

app.Run();