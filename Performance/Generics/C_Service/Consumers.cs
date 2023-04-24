public class InterfaceBased
{
    public static int Perform(
        ICalculator calculator,
        int a,
        int b
    )
    {
        var result = 0;
        for (int i = 0; i < 100; i++)
        {
            result += calculator.Add(a, b);
        }

        return result;
    }
}

public class GenericBased
{
    public static int Perform<T>(
        T calculator,
        int a,
        int b
    )
        where T : ICalculator
    {
        var result = 0;
        for (int i = 0; i < 100; i++)
        {
            result += calculator.Add(a, b);
        }

        return result;
    }
}