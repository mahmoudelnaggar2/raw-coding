using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Table>();

var app = builder.Build();

app.UseStaticFiles();

app.Use(async (ctx, next) =>
{
    if (!ctx.Request.Path.StartsWithSegments("/update"))
    {
        var memory = ctx.RequestServices.GetRequiredService<Table>();
        var god = memory[Table.GOD_OBJECT_REF];
        var ass = memory[Table.ASSEMBLY_COMPILER_REF];
        
        var routeRequestObjectRef = int.Parse(god.Fields[4].Value);
        var routeRequestObjectFieldRef = int.Parse(god.Fields[5].Value);

        var routeRequestObject = memory[routeRequestObjectRef];
        var routeCode = routeRequestObject.Fields[routeRequestObjectFieldRef].Value;
        var fn = (Func<Object, string, object[], Task<object>>)ass.Method[0].Delegate;
        await fn(routeRequestObject, routeCode, new object[] { ctx });
    }

    if (!ctx.Response.HasStarted)
    {
        await next(ctx);
    }
});

app.MapPut("/update/{id:int}", async (
    int id,
    Message msg,
    Table memory,
    HttpContext ctx
) =>
{
    var o = memory[Table.GOD_OBJECT_REF];

    if (new[] { 0, 1, 2 }.Contains(id))
    {
        DefaultUpdate();
        return;
    }


    var updateObj = int.Parse(o.Fields[2].Value);
    var updateObjField = int.Parse(o.Fields[3].Value);

    var ass = memory[Table.ASSEMBLY_COMPILER_REF];
    var fn = (Func<Object, string, object[], Task<object>>)ass.Method[0].Delegate;
    try
    {
        var @this = memory[updateObj];
        await fn(@this, @this.Fields[updateObjField].Value, new object[] { ctx });
    }
    catch
    {
        DefaultUpdate();
    }

    void DefaultUpdate()
    {
        memory[id].Fields[msg.Key] = new Field()
        {
            Name = msg.Name,
            Type = msg.Type,
            Value = msg.Value,
        };
    }
});

app.MapFallback(() => Results.Redirect("/index.html"));

app.Run();

public class Object
{
    public int Id { get; set; }
    public Dictionary<int, Field> Fields { get; set; } = new();
    public Dictionary<int, Method> Method { get; set; } = new();
}

public class Field
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}

public class Method
{
    public string Name { get; set; }
    [JsonIgnore] public Delegate Delegate { get; set; }
}

public class Message
{
    public int Key { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}