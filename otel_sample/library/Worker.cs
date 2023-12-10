using System.Diagnostics;

namespace library;

    public class Worker
    {
        private static readonly ActivitySource MyActivitySource = new ActivitySource("OTELSample.Library");

        public void Work()
        {
            using (var activity = MyActivitySource.StartActivity("Worker.Work"))
            {
                activity?.SetTag("foo", 1);
                activity?.SetTag("bar", "Hello, from lib!");
                activity?.SetTag("baz", new int[] { 1, 2, 3 });
                activity?.SetStatus(ActivityStatusCode.Ok);

                Thread.Sleep(100);

                System.Console.WriteLine("Did some work.");
            }
        }
    }
