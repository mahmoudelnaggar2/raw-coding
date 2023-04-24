using System.Xml;
using HtmlProgrammingLanguage.Core.Keywords;
using HtmlProgrammingLanguage.Core.Keywords.Arithmetic;

namespace HtmlProgrammingLanguage.Core;

public class Interpreter
{
    private readonly Action<string> _output;
    private readonly XmlDocument _sourceTree;

    public Interpreter(
        Action<string> output,
        string source
    )
    {
        _output = output;
        _sourceTree = new();
        _sourceTree.LoadXml(source);
    }

    public object Execute(IEnumerable<string> args)
    {
        var globalScope = new ExecutionScope(null, this);
        var runToLast = globalScope.EvaluateChildren(_sourceTree.ChildNodes).Last();
        var main = (Func<IEnumerable<object>, object>)globalScope.GetVariable("main");
        return main(args);
    }

    public IExpression ResolveNodeKeyword(
        ExecutionScope executionScope,
        XmlNode node
    ) => node switch
    {
        null => throw new("received null node"),

        { Name: "var" } => new Var(executionScope, node),
        { Name: "fn" } => new Fn(executionScope, node, this),

        { Name: "print" } => new Print(executionScope, node, _output),

        { Name: "add" } => new Arithmetic(executionScope, node, AOp.Add),
        { Name: "sub" } => new Arithmetic(executionScope, node, AOp.Sub),
        { Name: "mul" } => new Arithmetic(executionScope, node, AOp.Mul),
        { Name: "div" } => new Arithmetic(executionScope, node, AOp.Div),
        { Name: "mod" } => new Arithmetic(executionScope, node, AOp.Mod),

        { Name: "if" } => new If(executionScope, node),
        { Name: "eq" } => new BoolComparison(executionScope, node, BOp.Eq),

        { Name: "val" } => new Val(executionScope, node),
        { Name: "return" } => new Return(executionScope, node),
        _ => new Noop(),
    };
}