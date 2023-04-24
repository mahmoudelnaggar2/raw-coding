using StackExchange.Redis;

var number = new SavedNumber();
var connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1");
var db = connectionMultiplexer.GetDatabase();
var o = new { };

var promise = new TaskCompletionSource();
var semaphore = new SemaphoreSlim(1);
// var gunShot = semaphore.WaitAsync();
var mutex = new Mutex();
var work = async () =>
{
    await promise.Task;
    // await gunShot;
    // lock (o)
    // {
    //     number.IncrementAsync().GetAwaiter().GetResult();
    // }
    // mutex.WaitOne();
    // await number.IncrementAsync();
    // mutex.ReleaseMutex();
    // await semaphore.WaitAsync();
    // await number.IncrementAsync();
    // semaphore.Release();
    
    while (!await db.LockTakeAsync("number", "val", TimeSpan.FromSeconds(10)))
    {
        await Task.Delay(5);
    }

    try
    {
        await number.IncrementAsync();
    }
    catch
    {

    }
    finally
    {
        await db.LockReleaseAsync("number", "val");
    }
};
var tasks = new List<Task>();
for (int i = 0; i < 1000; i++)
{
    var t = work();
    tasks.Add(t);
}

// semaphore.Release();
promise.SetResult();
await Task.WhenAll(tasks);

// Console.WriteLine(number);