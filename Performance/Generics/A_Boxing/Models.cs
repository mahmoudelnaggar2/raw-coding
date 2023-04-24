public interface Interfaze
{
    string BespokeToString();
}

public class Clazz : Interfaze
{
    public string Value { get; set; }
    public string BespokeToString() => Value;
}


public struct Strakt : Interfaze
{
    public string Value { get; set; }
    public string BespokeToString() => Value;
}