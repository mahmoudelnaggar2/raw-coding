using BenchmarkDotNet.Attributes;

namespace LOHvsSOH;

public class LoopOver
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
    public long SOH()
    {
        long last = 0;

        for (int i = 0; i < 20000; i++)
        {
            last = _small[i % 2000];
        }

        return last;
    }

    [Benchmark()]
    public long LOH()
    {
        long last = 0;

        for (int i = 0; i < 20000; i++)
        {
            last = _large[i % 20000];
        }

        return last;
    }
}










/*
|     Method |     Mean |    Error |   StdDev |
|----------- |---------:|---------:|---------:|
|        SOH | 15.29 us | 0.014 us | 0.012 us |
|        LOH | 15.27 us | 0.013 us | 0.012 us |
*/