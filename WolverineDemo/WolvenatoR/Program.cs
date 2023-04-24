using MediatR;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(r => r.RegisterServicesFromAssemblyContaining<QueryMediatR>());

builder.Host.UseWolverine();

var app = builder.Build();

app.MapGet("/mediatr", (IMediator mediator) =>
    mediator.Send(new RequestMediatR(Guid.NewGuid().ToString()))
);

app.MapGet("/wolverine", (IMessageBus bus) =>
    bus.InvokeAsync<string>(new RequestWolverine(Guid.NewGuid().ToString()))
);

app.Run();