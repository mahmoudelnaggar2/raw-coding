public class RegularFactory
{
    public static int Create(
        ServiceType type,
        int number
    )
    {
        if (type == ServiceType.A)
        {
            return number * 5;
        }

        if (type == ServiceType.B)
        {
            return number * 5;
        }

        return 0;
    }
}

public class OptimizedFactory
{
    public static int Create<T>(int number) where T : IServiceType
    {
        if (typeof(T) == typeof(ServiceTypeA))
        {
            return number * 5;
        }

        if (typeof(T) == typeof(ServiceTypeB))
        {
            return number * 5;
        }

        return 0;
    }
}