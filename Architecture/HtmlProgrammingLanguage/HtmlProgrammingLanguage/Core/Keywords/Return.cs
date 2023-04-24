using System.Xml;

namespace HtmlProgrammingLanguage.Core.Keywords;

public class Return : IExpression
{
    private readonly ExecutionScope _scope;
    private readonly XmlNode _node;

    public Return(
        ExecutionScope scope,
        XmlNode node
    )
    {
        _scope = scope;
        _node = node;
    }

    public object Execute(IEnumerable<object> arguments)
    {
        return _scope.EvaluateChildren(_node.ChildNodes).First();
    }
}