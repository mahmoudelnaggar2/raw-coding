namespace HtmlProgrammingLanguage.Core;

public interface IExpression
{
    object Execute() => Execute(Enumerable.Empty<object>());
    object Execute(IEnumerable<object> arguments);
}