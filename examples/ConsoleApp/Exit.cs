using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp;

internal static class Exit
{
    public static Task WaitAsync()
    {
        var tcs = new TaskCompletionSource();
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            tcs.SetResult();
        };
        return tcs.Task;
    }

    public static void Wait()
    {
        using var manualResetEventSlim = new ManualResetEventSlim();
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            manualResetEventSlim.Set();
        };
        manualResetEventSlim.Wait();
    }
}

static class Cancellation
{
    public static CancellationToken CreateCancellationToken()
    {
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        return cts.Token;
    }
}
