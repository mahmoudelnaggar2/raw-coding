public static class ObjectService
{
    public static string Do(object something) => (something as Interfaze).BespokeToString();
}

public static class InterfaceService
{
    public static string Do(Interfaze something) => something.BespokeToString();
}

public static class GenericService
{
    public static string Do<T>(T something) where T : Interfaze => something.BespokeToString();
}