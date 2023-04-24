namespace HtmlProgrammingLanguage.Core.Keywords;

public class Noop : IExpression
{
    public object Execute(IEnumerable<object> arguments)
    {
        throw new NotImplementedException();
    }
}