using System.Threading.Channels;
using RuleEngine;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();
builder.Services.AddSingleton(Channel.CreateUnbounded<int>());

var app = builder.Build();

app.MapGet("/submit", (
    int num,
    IMessagePublisher publisher
) =>
{
    publisher.SendAsync(new NumberEmitted() { Number = num });
    return $"published: {num}";
});

app.MapGet("/result", (Channel<int> channel) =>
{
    var result = new List<int>();
    while (channel.Reader.TryRead(out var num))
    {
        result.Add(num);
    }

    return result;
});

app.Run();