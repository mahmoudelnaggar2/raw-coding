var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(co =>
{
    co.AddPolicy("name", pb =>
    {
        
    });
});

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:5018";
    ctx.Response.Headers["Access-Control-Expose-Headers"] = "some-custom-header";
    ctx.Response.Headers["some-custom-header"] = "secret!";

    if (HttpMethods.IsOptions(ctx.Request.Method))
    {
        ctx.Response.Headers["Access-Control-Allow-Headers"] = "my-a, my-b";
        ctx.Response.Headers["Access-Control-Allow-Methods"] = "POST, GET, OPTIONS, PUT";

        await ctx.Response.CompleteAsync();
        return;
    }
    
    await next();
});

app.UseCors("name");

app.MapGet("/", () => "Hello World!");
app.MapPost("/", () => "Hello World!");

app.Run();