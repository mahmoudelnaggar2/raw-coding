using BenchmarkDotNet.Attributes;

namespace LOHvsSOH;

public class Access
{
    private long[] _small;
    private long[] _large;

    [GlobalSetup]
    public void Setup()
    {
        _small = Enumerable.Range(0, 2000).Select(_ => Random.Shared.NextInt64()).ToArray();
        _large = Enumerable.Range(0, 20000).Select(_ => Random.Shared.NextInt64()).ToArray();
    }

    [Benchmark()]
    public long SOH() => _small[Random.Shared.Next() % 2000];
    
    [Benchmark()]
    public long LOH() => _large[Random.Shared.Next() % 20000];
    
    [Benchmark()]
    public long SOHStatic() => _small[1000];
    
    [Benchmark()]
    public long LOHStatic() => _large[1000];
}
















/*
|    Method |      Mean |     Error |    StdDev |    Median |
|---------- |----------:|----------:|----------:|----------:|
|       SOH | 5.5154 ns | 0.0291 ns | 0.0243 ns | 5.5190 ns |
|       LOH | 5.6106 ns | 0.0695 ns | 0.0650 ns | 5.5987 ns |
| SOHStatic | 0.0046 ns | 0.0068 ns | 0.0063 ns | 0.0021 ns |
| LOHStatic | 0.0219 ns | 0.0100 ns | 0.0094 ns | 0.0165 ns |
*/