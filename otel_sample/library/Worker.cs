using System.Diagnostics;

namespace library;

public class Worker
{
    private static readonly ActivitySource MyActivitySource =
        new ActivitySource("OTELSample.Library");

    public void Work()
    {
        using (var activity = MyActivitySource.StartActivity("Worker.Work"))
        {
            activity?.SetTag("foo", 1);
            activity?.SetTag("bar", "Hello, from lib!");
            activity?.SetTag("baz", new int[] { 1, 2, 3 });

            Thread.Sleep(100);

            Console.WriteLine("Did some work.");

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
    }
}
