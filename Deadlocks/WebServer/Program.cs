var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<Sem>();
builder.Services.AddSingleton(new SemaphoreSlim(1));

var app = builder.Build();

app.Map("/forgotten", async (Sem sem) =>
{
    await sem.WaitAsync();
    return "Hello World";
});

app.Map("/recur", async (
    SemaphoreSlim slim,
    IHttpClientFactory httpClientFactory
) =>
{
    var client = httpClientFactory.CreateClient();
    try
    {
        await slim.WaitAsync();

        var response = await client.GetAsync("https://localhost:7000/recur");
    }
    finally
    {
        slim.Release();
    }

    return "Hello World";
});

app.Map("/mutual", async (
    SemaphoreSlim slim,
    IHttpClientFactory httpClientFactory
) =>
{
    var client = httpClientFactory.CreateClient();
    try
    {
        await slim.WaitAsync();

        var response = await client.GetAsync("https://localhost:7000/mutual-1");
    }
    finally
    {
        slim.Release();
    }

    return "Hello World";
});


app.Map("/mutual-1", async (
    SemaphoreSlim slim,
    IHttpClientFactory httpClientFactory
) =>
{
    var client = httpClientFactory.CreateClient();
    try
    {
        await slim.WaitAsync();

        var response = await client.GetAsync("https://localhost:7000/mutual");
    }
    finally
    {
        slim.Release();
    }

    return "Hello World";
});

app.Run();

public class Sem : SemaphoreSlim
{
    public Sem() : base(1)
    {
    }
}