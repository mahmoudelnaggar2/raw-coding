using BenchmarkDotNet.Attributes;

namespace LOHvsSOH;

public class Allocation
{
    [Benchmark()]
    public long[] SOHSmaller()
    {
        long[] last = null;
        for (int i = 0; i < 100 * 60; i++)
        {
            last = new long[200];
        }

        return last;
    }
    
    [Benchmark()]
    public long[] SOH()
    {
        long[] last = null;
        for (int i = 0; i < 100 * 6; i++)
        {
            last = new long[2000];
        }

        return last;
    }
    
    [Benchmark()]
    public long[] SOHBigger()
    {
        long[] last = null;
        for (int i = 0; i < 100 * 3; i++)
        {
            last = new long[4000];
        }

        return last;
    }

    [Benchmark()]
    public long[] LOH()
    {
        long[] last = null;
        for (int i = 0; i < 100; i++)
        {
            last = new long[12000];
        }

        return last;
    }
}









/*
|     Method |     Mean |   Error |  StdDev |
|----------- |---------:|--------:|--------:|
| SOHSmaller | 295.9 us | 2.61 us | 2.44 us |
|        SOH | 264.9 us | 5.04 us | 6.00 us |
|  SOHBigger | 251.9 us | 4.84 us | 4.75 us |
|        LOH | 452.9 us | 4.36 us | 4.08 us |
*/