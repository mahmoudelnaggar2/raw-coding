using System.Xml;

namespace HtmlProgrammingLanguage.Core.Keywords.Arithmetic;

public enum AOp
{
    Add = 0,
    Sub = 1,
    Mul = 2,
    Div = 3,
    Mod = 4,
}

public class Arithmetic : IExpression
{
    private readonly ExecutionScope _scope;
    private readonly XmlNode _node;
    private readonly Func<int, int, int> _op;

    private static readonly Func<int, int, int>[] SOpMap = new[]
    {
        (
            int a,
            int b
        ) => a + b,
        (
            int a,
            int b
        ) => a - b,
        (
            int a,
            int b
        ) => a * b,
        (
            int a,
            int b
        ) => a / b,
        (
            int a,
            int b
        ) => a % b,
    };

    public Arithmetic(
        ExecutionScope scope,
        XmlNode node,
        AOp op
    )
    {
        _scope = scope;
        _node = node;
        _op = SOpMap[(int)op];
    }

    public object Execute(IEnumerable<object> arguments)
    {
        var result = _scope.EvaluateChildren(_node.ChildNodes)
            .Select(v => v switch
            {
                int n => n,
                string str when int.TryParse(str, out var n) => n,
                _ => throw new Exception($"expected number value, got: {v}")
            })
            .Aggregate(_op);

        return result;
    }
}