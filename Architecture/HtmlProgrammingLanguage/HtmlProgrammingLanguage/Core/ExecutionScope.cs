using System.Xml;
using HtmlProgrammingLanguage.Core.Keywords;

namespace HtmlProgrammingLanguage.Core;

public class ExecutionScope
{
    private readonly ExecutionScope? _parentScope;
    private readonly Interpreter _interpreter;
    private readonly Dictionary<string, object> _vars = new();

    public ExecutionScope(
        ExecutionScope? parentScope,
        Interpreter interpreter
    )
    {
        _parentScope = parentScope;
        _interpreter = interpreter;
    }

    public bool Return { get; set; }

    public IEnumerable<object> EvaluateChildren(XmlNodeList childExpressions)
    {
        for (int i = 0; i < childExpressions.Count; i++)
        {
            if(Return) yield break;
            
            var child = childExpressions.Item(i);
            if (child is XmlText xt) yield return xt.Value?.ReplaceLineEndings() ?? "";
            else if (child != null) yield return Evaluate(child);
        }
    }

    private object Evaluate(XmlNode node)
    {
        var exp = _interpreter.ResolveNodeKeyword(this, node);
        if (exp is Return)
        {
            var finalResult = exp.Execute();
            Return = true;
            return finalResult;
        }

        if (exp is not Noop) return exp.Execute();

        var variable = GetVariable(node.Name);
        if (variable is Func<IEnumerable<object>, object> fn)
        {
            return fn(EvaluateChildren(node.ChildNodes));
        }

        return variable;
    }

    public void SetVariable(
        string key,
        object v
    ) => _vars.Add(key, v);

    public object GetVariable(string key)
    {
        if (_vars.TryGetValue(key, out var v))
        {
            return v;
        }

        if (_parentScope == null)
        {
            throw new Exception($"invalid variable {key}");
        }

        return _parentScope.GetVariable(key);
    }
}