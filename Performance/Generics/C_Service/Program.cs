using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchServices>();


[MemoryDiagnoser]
public class BenchServices
{
    public Implemintation _service = new Implemintation();

    [Benchmark]
    public int Interface() => InterfaceBased.Perform(_service, 10, 2);

    [Benchmark]
    public int Generic() => GenericBased.Perform(_service, 10, 2);
}

/*
|    Method |     Mean |   Error |  StdDev | Allocated |
|---------- |---------:|--------:|--------:|----------:|
| Interface | 131.7 ns | 0.20 ns | 0.18 ns |         - |
|   Generic | 132.6 ns | 0.33 ns | 0.28 ns |         - |
*/