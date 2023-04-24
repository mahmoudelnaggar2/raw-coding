using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchStructs>();


[MemoryDiagnoser]
public class BenchObjects
{
    private Clazz _o = new Clazz();

    [GlobalSetup]
    public void Setup()
    {
        _o = new Clazz() { Value = "Hello World" };
    }
    
    [Benchmark]
    public string Object() => ObjectService.Do(_o);

    [Benchmark]
    public string Interface() => InterfaceService.Do(_o);

    [Benchmark]
    public string Generic() => GenericService.Do(_o);
}
/*
|    Method |      Mean |     Error |    StdDev | Allocated |
|---------- |----------:|----------:|----------:|----------:|
|    Object | 0.6406 ns | 0.0057 ns | 0.0051 ns |         - |
| Interface | 0.6524 ns | 0.0133 ns | 0.0111 ns |         - |
|   Generic | 0.9540 ns | 0.0079 ns | 0.0074 ns |         - |
 */

[MemoryDiagnoser]
public class BenchStructs
{
    private Strakt _o;

    [GlobalSetup]
    public void Setup()
    {
        _o = new Strakt() { Value = "Hello World" };
    }

    [Benchmark]
    public string Object() => ObjectService.Do(_o);

    [Benchmark]
    public string Interface() => InterfaceService.Do(_o);

    [Benchmark]
    public string Generic() => GenericService.Do(_o);
}

/*
|    Method |      Mean |     Error |    StdDev |    Median |   Gen0 | Allocated |
|---------- |----------:|----------:|----------:|----------:|-------:|----------:|
|    Object | 5.5342 ns | 0.1036 ns | 0.0969 ns | 5.5311 ns | 0.0038 |      24 B |
| Interface | 5.4464 ns | 0.0868 ns | 0.0812 ns | 5.4649 ns | 0.0038 |      24 B |
|   Generic | 0.0027 ns | 0.0052 ns | 0.0049 ns | 0.0000 ns |      - |         - |
*/