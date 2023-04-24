namespace MediatR;


public record RequestWolverine(string Something);


public class QueryWolverineHandler
{
    public string Handle(RequestWolverine request)
    {
        return $"Hello World {request.Something}";
    }
}