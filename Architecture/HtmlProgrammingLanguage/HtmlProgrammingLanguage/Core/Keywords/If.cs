using System.Xml;

namespace HtmlProgrammingLanguage.Core.Keywords;

public class If : IExpression
{
    private readonly ExecutionScope _executionScope;
    private readonly XmlNode _node;

    public If(
        ExecutionScope executionScope,
        XmlNode node
    )
    {
        _executionScope = executionScope;
        _node = node;
    }

    public object Execute(IEnumerable<object> arguments)
    {
        var instructions = _executionScope.EvaluateChildren(_node.ChildNodes).GetEnumerator();

        if (!instructions.MoveNext())
        {
            throw new Exception($"no condition statement specified for {_node.OuterXml}");
        }

        var condition = instructions.Current switch
        {
            bool b => b,
            > 0 => true,
            string { Length: > 0 } => true,
            _ => throw new Exception($"condition didn't receive a bool value {_node.OuterXml}")
        };

        if (condition)
        {
            while (instructions.MoveNext())
            {
            }

            return instructions.Current;
        }

        return Defaults.Empty;
    }
}