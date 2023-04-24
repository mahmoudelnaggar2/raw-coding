using HtmlProgrammingLanguage.Core;

var html = File.ReadAllText(args[0]);

new Interpreter(
    Console.WriteLine,
    html
).Execute(args.Skip(1));