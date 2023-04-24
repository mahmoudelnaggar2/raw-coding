using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class Table : Dictionary<int, Object>
{
    public const int GOD_OBJECT_REF = 0;
    public const int ASSEMBLY_COMPILER_REF = 1;
    public const int UPDATE_FIELD_REF = 2;
    public const int ROUTE_REQUEST_REF = 3;

    public Table()
    {
        this[GOD_OBJECT_REF] = new Object()
        {
            Id = GOD_OBJECT_REF,
            Fields = new()
            {
                { 0, new Field() { Name = "path", Type = "value", Value = "/update" } },
                { 1, new Field() { Name = "method", Type = "value", Value = "PUT" } },
                { 2, new Field() { Name = "update-object", Type = "value", Value = UPDATE_FIELD_REF.ToString() } },
                { 3, new Field() { Name = "update-object-field", Type = "value", Value = "1" } },
                {
                    4,
                    new Field() { Name = "route-request-object", Type = "value", Value = ROUTE_REQUEST_REF.ToString() }
                },
                { 5, new Field() { Name = "route-request-object-field", Type = "value", Value = "1" } },
            },
        };
        this[ASSEMBLY_COMPILER_REF] = new Object()
        {
            Id = ASSEMBLY_COMPILER_REF,
            Fields = new()
            {
                { 0, new Field() { Name = "description", Type = "value", Value = "C# Compiler" } },
            },
            Method = new()
            {
                {
                    0,
                    new Method()
                    {
                        Name = "invoke",
                        Delegate = (Func<Object, string, object[], Task<object>>)AssHelper.Execute
                    }
                }
            }
        };
        this[UPDATE_FIELD_REF] = new Object()
        {
            Id = UPDATE_FIELD_REF,
            Fields = new()
            {
                {
                    0,
                    new Field()
                    {
                        Name = "description", Type = "value", Value = "Default update function"
                    }
                },
                {
                    1, new Field()
                    {
                        Name = "dynamic", Type = "textarea", Value = """
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public class Function {
    public static Task Handle(Object obj, HttpContext ctx){
        var id = int.Parse(ctx.Request.Path.Value.Split('/').Last());

        var msg = await ctx.Request.ReadFromJsonAsync<Message>();
        var memory = ctx.RequestServices.GetRequiredService<Table>();
        memory[id].Fields[msg.Key] = new Field()
        {
            Name = msg.Name,
            Type = msg.Type,
            Value = msg.Value,
        };

        return id;
    }
}
"""
                    }
                },
            }
        };
        this[ROUTE_REQUEST_REF] = new Object()
        {
            Id = ROUTE_REQUEST_REF,
            Fields = new()
            {
                {
                    0,
                    new Field()
                    {
                        Name = "description", Type = "value", Value = "Routing "
                    }
                },
                {
                    1, new Field()
                    {
                        Name = "dynamic", Type = "textarea", Value = """
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public class Function {
    public static async Task Handle(Object obj, HttpContext ctx){
        var memory = ctx.RequestServices.GetRequiredService<Table>();
        var target = memory.Values.FirstOrDefault(x =>
            x.Fields.Values.Any(y => y.Name == "path" && ctx.Request.Path.StartsWithSegments(y.Value))
            && x.Fields.Values.Any(y => y.Name == "method" && y.Value == ctx.Request.Method));

        var handle = target.Method.Values.FirstOrDefault(x => x.Name == "handle");
        if (handle != null)
        {
            var result = handle.Delegate.DynamicInvoke(ctx);
            if (result is Task<object> tr)
            {
                var invokeResult = await tr;
                await ctx.Response.WriteAsJsonAsync(invokeResult);
            }
            else if (result is Task t)
            {
                await t;
                await ctx.Response.CompleteAsync();
            }
            else
            {
                await ctx.Response.WriteAsJsonAsync(result);
            }
            return;
        }

        var staticHandle = target.Fields.Values.FirstOrDefault(x => x.Name == "static");
        if (staticHandle != null)
        {
            await ctx.Response.WriteAsync(staticHandle.Value);
            return;
        }

        var dynamicHandle = target.Fields.Values.FirstOrDefault(x => x.Name == "dynamic");
        if (dynamicHandle != null)
        {
            var code = dynamicHandle.Value;
            var ass = memory[Table.ASSEMBLY_COMPILER_REF];
            var fn = (Func<Object, string, object[], Task<object>>)ass.Method[0].Delegate;

            var result = await fn(target, code, new object[] { ctx });
            if (result is Task t)
            {
                await t;
            }
        }
    }
}
"""
                    }
                },
            }
        };
        this[4] = new Object()
        {
            Id = 4,
            Fields = new()
            {
                { 0, new Field() { Name = "path", Type = "value", Value = "/create" } },
                { 1, new Field() { Name = "method", Type = "value", Value = "POST" } },
            },
            Method = new()
            {
                {
                    0,
                    new Method()
                    {
                        Name = "handle",
                        Delegate = (Func<HttpContext, int>)(ctx =>
                        {
                            var memory = ctx.RequestServices.GetRequiredService<Table>();
                            var id = memory.Count == 0 ? 0 : memory.Keys.Max() + 1;
                            var obj = new Object() { Id = id };
                            memory.Add(id, obj);
                            return id;
                        }),
                    }
                }
            }
        };
        this[5] = new Object()
        {
            Id = 5,
            Fields = new()
            {
                { 0, new Field() { Name = "path", Type = "value", Value = "/gc" } },
                { 1, new Field() { Name = "method", Type = "value", Value = "POST" } },
            },
            Method = new()
            {
                {
                    0,
                    new Method()
                    {
                        Name = "handle",
                        Delegate =
                            (Action<HttpContext>)(ctx =>
                            {
                                var mem = ctx.RequestServices.GetRequiredService<Table>();
                                foreach (var k in mem.Keys.Where(k => k > 9)) mem.Remove(k);
                            }),
                    }
                }
            }
        };
        this[6] = new Object()
        {
            Id = 6,
            Fields = new()
            {
                { 0, new Field() { Name = "path", Type = "value", Value = "/table" } },
                { 1, new Field() { Name = "method", Type = "value", Value = "GET" } },
            },
            Method = new()
            {
                {
                    0,
                    new Method()
                    {
                        Name = "handle",
                        Delegate = (Func<HttpContext, Table>)(ctx => ctx.RequestServices.GetRequiredService<Table>()),
                    }
                }
            }
        };

        this[7] = new Object()
        {
            Id = 7,
            Fields = new()
            {
                { 0, new Field() { Name = "path", Type = "value", Value = "/clone" } },
                { 1, new Field() { Name = "method", Type = "value", Value = "PUT" } },
            },
            Method = new()
            {
                {
                    0,
                    new Method()
                    {
                        Name = "handle",
                        Delegate = (Func<HttpContext, int>)(ctx =>
                        {
                            var id = int.Parse(ctx.Request.Path.Value.Split('/').Last());
                            var memory = ctx.RequestServices.GetRequiredService<Table>();

                            var newId = memory.Count == 0 ? 0 : memory.Keys.Max() + 1;
                            var obj = new Object() { Id = newId };
                            foreach (var (k, v) in memory[id].Fields)
                                obj.Fields.Add(k, v);

                            memory.Add(newId, obj);
                            return newId;
                        }),
                    }
                }
            }
        };
        this[8] = new Object()
        {
            Id = 8,
            Fields = new()
            {
                { 0, new Field() { Name = "path", Type = "value", Value = "/test-static" } },
                { 1, new Field() { Name = "method", Type = "value", Value = "GET" } },
                {
                    2, new Field()
                    {
                        Name = "static", Type = "textarea", Value = """
<html>
<head></head>
<body>
Hello World
</body>
</html>
"""
                    }
                },
            },
        };
        this[9] = new Object()
        {
            Id = 9,
            Fields = new()
            {
                { 0, new Field() { Name = "path", Type = "value", Value = "/test-dynamic" } },
                { 1, new Field() { Name = "method", Type = "value", Value = "GET" } },
                {
                    2, new Field()
                    {
                        Name = "dynamic", Type = "textarea", Value = """
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class Function {
    public static Task Handle(Object obj, HttpContext ctx){
        return ctx.Response.WriteAsync("<h1>Hello World from dynamic code</h1>");
    }
}
"""
                    }
                },
            },
        };
    }
}