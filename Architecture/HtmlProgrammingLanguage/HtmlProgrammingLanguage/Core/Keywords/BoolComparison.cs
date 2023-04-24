using System.Xml;

namespace HtmlProgrammingLanguage.Core.Keywords;

public enum BOp
{
    Eq = 0,
}
public class BoolComparison : IExpression
{
    private readonly ExecutionScope _scope;
    private readonly XmlNode _node;

    private readonly Func<object, object, bool> _op;

    private static readonly Func<object, object, bool>[] SOpMap = new[]
    {
        (object a, object b) => (a,b) switch
        {
            (bool, bool) => a.Equals(b),
            (int, int) => a.Equals(b),
            (string, string) => a.Equals(b),
            (int x, string s) => int.TryParse(s, out var y) && x == y,
            (string s, int y) => int.TryParse(s, out var x) && x == y,
            _ => false
        }
    };

    public BoolComparison(
        ExecutionScope scope,
        XmlNode node,
        BOp op
    )
    {
        _scope = scope;
        _node = node;
        _op = SOpMap[(int)op];
    }

    public object Execute(IEnumerable<object> arguments)
    {
        var values = _scope.EvaluateChildren(_node.ChildNodes).ToList();
        var left = values[0];
        var right = values[1];
        return _op(left, right);
    }
}