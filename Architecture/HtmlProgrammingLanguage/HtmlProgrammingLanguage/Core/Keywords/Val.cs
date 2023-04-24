using System.Xml;

namespace HtmlProgrammingLanguage.Core.Keywords;

public class Val : IExpression
{
    private readonly ExecutionScope _scope;
    private readonly XmlNode _node;

    public Val(
        ExecutionScope scope,
        XmlNode node
    )
    {
        _scope = scope;
        _node = node;
    }

    public object Execute(IEnumerable<object> arguments)
    {
        return _node.Attributes?["is"]?.Value ?? "";
    }
}