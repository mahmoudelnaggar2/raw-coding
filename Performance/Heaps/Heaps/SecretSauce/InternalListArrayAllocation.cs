using System.Reflection;

namespace Heaps;

public class InternalListArrayAllocation
{
    public static FieldInfo _field = typeof(List<ComplexObject>)
        .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
        .First(x => x.Name == "_items");


    public static string Handle()
    {
        int tippingPoint = (85000 / sizeof(long)) - 3;
        int listTippingPoint = 4;
        while (listTippingPoint < tippingPoint)
        {
            listTippingPoint *= 2;
        }

        listTippingPoint /= 2;

        Console.WriteLine($"array: {tippingPoint}, list tipping point: {listTippingPoint}");
        var list = new List<ComplexObject>();

        list.AddRange(
            Enumerable.Range(0, listTippingPoint)
                .Select(i => new ComplexObject() { Id = i, Name = i.ToString() })
        );


        var internalArray = _field.GetValue(list);
        Console.WriteLine($"internalArray in gen:{GC.GetGeneration(internalArray)}");
        list.Add(new ComplexObject());
        Console.WriteLine($"internalArray in gen:{GC.GetGeneration(internalArray)}");
        
        internalArray = _field.GetValue(list);
        Console.WriteLine($"new internalArray in gen:{GC.GetGeneration(internalArray)}");

        return "ok";
    }
}