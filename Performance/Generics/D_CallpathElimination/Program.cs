// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchElimination>();


[MemoryDiagnoser]
public class BenchElimination
{
    [Benchmark]
    public int ObjectGetType() => ObjectService.GetType(5);

    [Benchmark]
    public int GenericIs() => GenericService.Is(5);
    
    [Benchmark]
    public int GenericTypeOf() => GenericService.TypeOf(5);

    [Benchmark]
    public int Direct() => DirectService.Get(5);
}
/*
|        Method |      Mean |     Error |    StdDev |    Median |   Gen0 | Allocated |
|-------------- |----------:|----------:|----------:|----------:|-------:|----------:|
| ObjectGetType | 2.2184 ns | 0.0165 ns | 0.0155 ns | 2.2141 ns | 0.0038 |      24 B |
|     GenericIs | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |      - |         - |
| GenericTypeOf | 0.0004 ns | 0.0007 ns | 0.0006 ns | 0.0000 ns |      - |         - |
|        Direct | 0.0019 ns | 0.0020 ns | 0.0019 ns | 0.0020 ns |      - |         - |
*/