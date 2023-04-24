var result = new Result();

SemaphoreSlim f1 = new(1);
SemaphoreSlim f2 = new(1);

while (true)
{
    var t1 = Task.Run(async () =>
    {
        await f1.WaitAsync();
        await Task.Delay(1000);
        await f2.WaitAsync();
        result.Number++;
    });

    var t2 = Task.Run(async () =>
    {
        await f2.WaitAsync();
        await Task.Delay(1000);
        await f1.WaitAsync();
        result.Number++;
    });
    Console.WriteLine(result.Number);
}

Console.WriteLine("Hello World");

public class Result
{
    public int Number { get; set; }
}