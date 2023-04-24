using System.Xml;

namespace HtmlProgrammingLanguage.Core.Keywords;

public class Fn : IExpression
{
    private readonly ExecutionScope _executionScope;
    private readonly XmlNode _node;
    private readonly Interpreter _interpreter;

    public Fn(
        ExecutionScope executionScope,
        XmlNode node,
        Interpreter interpreter
    )
    {
        _executionScope = executionScope;
        _node = node;
        _interpreter = interpreter;
    }

    public object Execute(IEnumerable<object> arguments)
    {
        var attr = _node.Attributes?.GetNamedItem("name");

        if (attr == null) throw new Exception($"function missing name in {_node.OuterXml}");
        if (attr.Value == null) throw new Exception($"function missing name identifier in {_node.OuterXml}");

        _executionScope.SetVariable(attr.Value, (IEnumerable<object> args) =>
        {
            var functionScope = new ExecutionScope(_executionScope, _interpreter);
            var argsArray = args.ToArray();
            var parametersAttr = _node.Attributes?.GetNamedItem("parameters");
            if (parametersAttr != null)
            {
                var parameterNames = parametersAttr.Value?.Split(',').Select(x => x.Trim()).ToArray() ?? throw new Exception("no parameters specified");

                for (var i = 0; i < parameterNames.Length; i++)
                {
                    var pName = parameterNames[i];
                    try
                    {
                        var arg = argsArray[i];
                        functionScope.SetVariable(pName, arg);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new($"missing argument {pName}");
                    }
                }
            }

            var functionReturnValue = functionScope.EvaluateChildren(_node.ChildNodes).Last();
            return functionReturnValue;
        });

        // function definition return value
        return Defaults.Empty;
    }
}