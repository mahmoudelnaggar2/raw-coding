namespace Simulation;

public class ThreadBased
{
    private static object _o = new { };
    private static Mutex _m = new Mutex();

    public static void TestNumbers()
    {
        var number = 0;
        Test(() =>
        {
            // sync with lock
            // lock (_o)
            // {
            //     number++;
            // }

            // sync with mutex
            // _m.WaitOne();
            // number++;
            // _m.ReleaseMutex();

            number++;
        }, 1000);
        Console.WriteLine(number);
    }

    public static void Test(
        Action action,
        int times
    )
    {
        var semaphore = new SemaphoreSlim(0);
        var gunShot = semaphore.WaitAsync();
        ThreadStart work = () =>
        {
            gunShot.GetAwaiter().GetResult();
            action();
        };
        var threads = new List<Thread>();
        for (int i = 0; i < times; i++)
        {
            var t = new Thread(work);
            t.Start();
            threads.Add(t);
        }

        Thread.Sleep(100); // wait for thread to reach gunshot (be weary of threads going idle)
        semaphore.Release();
        threads.ForEach(_ => _.Join());
    }
}