public interface ICalculator
{
    public int Add(
        int number,
        int count
    );
}

public class Implemintation : ICalculator
{
    public int Add(
        int a,
        int b
    )
    {
        return a + b;
    }
}