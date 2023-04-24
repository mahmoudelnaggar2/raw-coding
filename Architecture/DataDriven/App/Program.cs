using System.Text.Json;
using App;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => "Hello World!");

app.MapPost("/compute", (Input input) =>
{
    var container = new Container(input.Number);
    // var opsList = JsonSerializer.Deserialize<List<Ops>>(input.OperationDescription);
    var opsList = input.OperationDescription.Split('\n')
        .Select(x => x.Split(' '))
        .Select(x => new Ops(x[0], int.Parse(x[1])));
    foreach (var (name, number) in opsList)
    {
        if (name == "Add")
        {
            container = new Add() { Number = number }.Perform(container);
        }
        else if (name == "Sub")
        {
            container = new Sub() { Number = number }.Perform(container);
        }
        else if (name == "Mul")
        {
            container = new Mul() { Number = number }.Perform(container);
        }
        else if (name == "Div")
        {
            container = new Div() { Number = number }.Perform(container);
        }
    }

    return container;
});

app.Run();

public record Input(int Number, string OperationDescription);

public record Ops(string Name, int Number);

// [
// {"Name": "Mul", "Number": 10},
// {"Name": "Add", "Number": 5}
// ]