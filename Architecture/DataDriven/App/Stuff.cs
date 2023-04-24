namespace App;

public record Container(int Number);

public class Add
{
    public int Number { get; set; }
    public Container Perform(Container input) => input with { Number = input.Number + Number };
}

public class Sub
{
    public int Number { get; set; }
    public Container Perform(Container input) => input with { Number = input.Number - Number };
}

public class Mul
{
    public int Number { get; set; }
    public Container Perform(Container input) => input with { Number = input.Number * Number };
}

public class Div
{
    public int Number { get; set; }
    public Container Perform(Container input) => input with { Number = input.Number / Number };
}

public class Combination1
{
    public Container Perform(Container input)
    {
        input = new Add() { Number = 1 }.Perform(input);
        return new Mul() { Number = 5 }.Perform(input);
    }
}

public class Combination2
{
    public Container Perform(Container input)
    {
        input = new Sub() { Number = 5 }.Perform(input);
        input = new Add() { Number = 1 }.Perform(input);
        return new Div() { Number = 2 }.Perform(input);
    }
}