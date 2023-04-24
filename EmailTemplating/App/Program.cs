using Fluid;
using Marten;
using Marten.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(x =>
{
    x.Connection("host=127.0.0.1;port=5666;database=email;user id=postgres;password=password;");
});
builder.Services.AddSingleton<FluidParser>();

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => "Hello World!");
app.MapPost("/create-template", async (EmailTemplate email, IDocumentSession session) =>
{
    session.Store(email);
    await session.SaveChangesAsync();
    return "ok";
});

app.MapPost("/create-generic", async (Generic gen, IDocumentSession session) =>
{
    session.Store(gen);
    await session.SaveChangesAsync();
    return "ok";
});

app.MapPost("/create-config", async (EmailConfig config, IDocumentSession session) =>
{
    session.Store(config);
    await session.SaveChangesAsync();
    return "ok";
});

app.MapGet("/test/{genId:guid}", async (Guid genId, FluidParser parser, IQuerySession session) =>
{
    var gen = await session.LoadAsync<Generic>(genId);
    EmailTemplate emailTemplate = null;
    
    var _ = await session.Query<EmailConfig>()
        .Include<EmailTemplate>(x => x.TemplateId, t => emailTemplate = t)
        .FirstOrDefaultAsync(x => x.Type == gen.Bag["type"]);

    if (parser.TryParse(emailTemplate.Template, out var template, out var error))
    {
        var context = new TemplateContext(gen.Bag);

        return template.Render(context);
    }

    return "bad";
});

app.Run();

public record EmailConfig(Guid Id, string Type, Guid TemplateId);
public record Generic(Guid Id, Dictionary<string,string> Bag);
public record EmailTemplate(Guid Id, string Template);