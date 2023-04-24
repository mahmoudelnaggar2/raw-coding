using System.Text;
using System.Xml;

namespace HtmlProgrammingLanguage.Core.Keywords;

public class Print : IExpression
{
    private readonly ExecutionScope _scope;
    private readonly XmlNode _node;
    private readonly Action<string> _output;

    public Print(
        ExecutionScope scope,
        XmlNode node,
        Action<string> output
    )
    {
        _scope = scope;
        _node = node;
        _output = output;
    }

    public object Execute(IEnumerable<object> arguments)
    {
        var sb = new StringBuilder();
        foreach (var val in _scope.EvaluateChildren(_node.ChildNodes))
        {
            if (val is string s)
            {
                sb.Append(s);
            }
            else if(val != Defaults.Empty)
            {
                sb.Append(val.ToString() ?? "");
            }
        }
       
        _output(sb.ToString());
       
        return Defaults.Empty;
    }
}