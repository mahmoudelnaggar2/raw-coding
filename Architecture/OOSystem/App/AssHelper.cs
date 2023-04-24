using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyInjection;

public class AssHelper
{
    public static async Task<object> Execute(
        Object obj,
        string functionCode,
        object[] args
    )
    {
        var tree = CSharpSyntaxTree.ParseText(functionCode);

        var coreAssemblyLocation = typeof(object).Assembly.Location;
        var baseAssemblyPath = Path.GetDirectoryName(coreAssemblyLocation);
        Console.WriteLine(coreAssemblyLocation);
        var compilation = CSharpCompilation.Create(
            Guid.NewGuid().ToString(),
            syntaxTrees: new[] { tree },
            references: new MetadataReference[]
            {
                MetadataReference.CreateFromFile(coreAssemblyLocation),
                MetadataReference.CreateFromFile(Path.Combine(baseAssemblyPath, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(baseAssemblyPath, "System.Collections.dll")),
                MetadataReference.CreateFromFile(Path.Combine(baseAssemblyPath, "System.Linq.dll")),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(HttpContext).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Dictionary<,>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IServiceProvider).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ServiceProviderServiceExtensions).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(HttpResponseJsonExtensions).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Program).Assembly.Location),
            },
            options: new(
                OutputKind.DynamicallyLinkedLibrary
            )
        );

        using var ms = new MemoryStream();
        var emitResult = compilation.Emit(ms);
        if (!emitResult.Success)
        {
            throw new Exception(JsonSerializer.Serialize(new
            {
                errors = emitResult.Diagnostics.Select(x => x.GetMessage()), code = functionCode,
            }));
        }

        var (alcRef, res) = await Process(obj, ms, args);

        for (int i = 0; i < 10 && alcRef.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        Console.WriteLine($"Unloading Successful: {obj.Id}:{!alcRef.IsAlive}");
        return res;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<(WeakReference, object?)> Process(
        Object obj,
        MemoryStream ms,
        object[] args
    )
    {
        ms.Position = 0;
        object? result = null;
        var loadContext = new AssemblyLoadContext(Guid.NewGuid().ToString(), isCollectible: true);
        try
        {
            var assembly = loadContext.LoadFromStream(ms);

            var func = assembly.GetType("Function");
            var method = func.GetMethods().First(x => x.Name == "Handle");
            result = method.Invoke(null, new object[] { obj }.Concat(args).ToArray());
        }
        finally
        {
            loadContext.Unload();
        }

        return (new WeakReference(loadContext), result);
    }
}