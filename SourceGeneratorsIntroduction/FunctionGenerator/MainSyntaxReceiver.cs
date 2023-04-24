using Microsoft.CodeAnalysis;

namespace FunctionGenerator;

public class MainSyntaxReceiver : ISyntaxReceiver
{
    public DefinitionAggregate Definitions { get; } = new();
    public GivethsAggregate Giveths { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        Definitions.OnVisitSyntaxNode(syntaxNode);
        Giveths.OnVisitSyntaxNode(syntaxNode);
    }
}