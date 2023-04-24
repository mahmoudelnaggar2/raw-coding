using System.Text;
using System.Xml;

namespace HtmlProgrammingLanguage.Core.Keywords;

public class Var : IExpression
{
    private readonly ExecutionScope _scope;
    private readonly XmlNode _node;

    public Var(
        ExecutionScope scope,
        XmlNode node
    )
    {
        _scope = scope;
        _node = node;
    }

    public object Execute(IEnumerable<object> arguments)
    {
        var attr = _node.Attributes?.GetNamedItem("name");

        if (attr == null) throw new Exception($"variable missing name in {_node.OuterXml}");
        if (attr.Value == null) throw new Exception($"variable missing name identifier in {_node.OuterXml}");
        
        var sb = new StringBuilder();
        foreach (var val in _scope.EvaluateChildren(_node.ChildNodes))
        {
            if (val is string s)
            {
                sb.Append(s);
            }
            else
            {
                sb.Append(val.ToString() ?? "");
            }
        }
        
        _scope.SetVariable(attr.Value, sb.ToString());

        return Defaults.Empty;
    }
}