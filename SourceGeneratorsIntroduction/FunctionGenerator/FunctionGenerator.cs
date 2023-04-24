using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace FunctionGenerator;

[Generator]
public class FunctionGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (MainSyntaxReceiver)context.SyntaxReceiver;
        foreach (var giveth in receiver.Giveths.Captures)
        {
            var def = receiver.Definitions.Captures.FirstOrDefault(x => x.Key == giveth.TargetImplementation);

            var output = giveth.Class
                .WithMembers(new(CreateGivethMethod(giveth.Method, def.Method)))
                .NormalizeWhitespace();
            
            context.AddSource($"{giveth.Class.Identifier.Text}.g.cs", output.GetText(Encoding.UTF8));
        }
    }

    private MethodDeclarationSyntax CreateGivethMethod(MethodDeclarationSyntax givethMethod, MethodDeclarationSyntax def)
    {
        return MethodDeclaration(givethMethod.ReturnType, givethMethod.Identifier)
            .WithModifiers(givethMethod.Modifiers)
            .WithBody(def.Body);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
    }
}