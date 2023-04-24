using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchElimination>();


public class BenchElimination
{
    [Params(ServiceType.A)] public ServiceType Type = ServiceType.A;

    [Benchmark]
    public int Regular() => RegularFactory.Create(Type, 100);

    [Benchmark]
    public int Optimized() => OptimizedFactory.Create<ServiceTypeA>(100);
}

/*
|    Method | Type |      Mean |     Error |    StdDev |
|---------- |----- |----------:|----------:|----------:|
|   Regular |    A | 0.1096 ns | 0.0024 ns | 0.0021 ns |
| Optimized |    A | 0.0048 ns | 0.0022 ns | 0.0021 ns |
*/