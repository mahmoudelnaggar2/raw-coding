// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchLists>();


[MemoryDiagnoser]
public class BenchLists
{
    [Params(100, 1000, 10000)] public int Count;


    [Benchmark]
    public string ArrayListWrite()
    {
        var list = new ArrayList();
        for (int i = 0; i < Count; i++)
        {
            list.Add(new Dummy());
        }

        return JsonSerializer.Serialize(list);
    }

    [Benchmark]
    public string ListWrite()
    {
        var list = new List<Dummy>();
        for (int i = 0; i < Count; i++)
        {
            list.Add(new Dummy());
        }

        return JsonSerializer.Serialize(list);
    }
}

public class Dummy
{
}

/*
|         Method | Count |       Mean |      Error |     StdDev |    Gen0 |    Gen1 |    Gen2 | Allocated |
|--------------- |------ |-----------:|-----------:|-----------:|--------:|--------:|--------:|----------:|
| ArrayListWrite |   100 |   6.396 us |  0.0600 us |  0.0561 us |  0.8850 |  0.0076 |       - |   5.46 KB |
|      ListWrite |   100 |   4.807 us |  0.0121 us |  0.0101 us |  0.8774 |  0.0076 |       - |    5.4 KB |
| ArrayListWrite |  1000 |  62.254 us |  1.2057 us |  1.3402 us |  7.4463 |  0.6104 |       - |   45.9 KB |
|      ListWrite |  1000 |  46.996 us |  0.6615 us |  0.5523 us |  7.4463 |  0.6104 |       - |  45.84 KB |
| ArrayListWrite | 10000 | 992.831 us | 14.2333 us | 12.6174 us | 82.0313 | 41.0156 | 41.0156 | 549.69 KB |
|      ListWrite | 10000 | 660.487 us |  9.4492 us |  8.3764 us | 83.0078 | 41.0156 | 41.0156 | 549.63 KB |
*/