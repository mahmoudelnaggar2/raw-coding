using System.Threading.Channels;
using Wolverine;

namespace RuleEngine;

public class EmittedHandler
{
    public static object Handle(NumberEmitted message)
    {
        return message.Number switch
        {
            0 => new Add { Number = message.Number },
            1 => new Sub { Number = message.Number },
            2 => new Mul { Number = message.Number },
            3 => new Div { Number = message.Number },
            _ => new Add { Number = message.Number },
        };
    }
}

public class AddHandler
{
    public static ValueTask Handle(Add message, Channel<int> channel)
    {
        var result = message.Number + 5;
        return channel.Writer.WriteAsync(result);
    }
}

public class SubHandler
{
    public static ValueTask Handle(Sub message, Channel<int> channel)
    {
        var result = message.Number - 5;
        return channel.Writer.WriteAsync(result);
    }
}

public class MulHandler
{
    public static ValueTask Handle(Mul message, Channel<int> channel)
    {
        var result = message.Number * 5;
        return channel.Writer.WriteAsync(result);
    }
}

public class DivHandler
{
    public static ValueTask Handle(Div message, Channel<int> channel)
    {
        var result = message.Number / 5;
        return channel.Writer.WriteAsync(result);
    }
}