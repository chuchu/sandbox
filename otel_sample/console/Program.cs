// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using library;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

internal class Program
{
    private static readonly ActivitySource MyActivitySource = new ActivitySource("OTELSample.Console");
    
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                serviceName: "OTELSampleConsole",
                serviceVersion: "1.0.0"))
            .AddSource("OTELSample.Console", "OTELSample.Library")
            // .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter()
            .Build();

        using (var activity = MyActivitySource.StartActivity("Say Hello"))
        {
            activity?.SetTag("foo", 1);
            activity?.SetTag("bar", "Hello, World!");
            activity?.SetTag("baz", new int[] { 1, 2, 3 });
            activity?.SetStatus(ActivityStatusCode.Ok);

            Thread.Sleep(100);

            Worker w = new Worker();
            w.Work();

            Thread.Sleep(200);

            InternalWorker();

            using (var client = new HttpClient())
            {
                using (var requestMessage = new HttpRequestMessage(
                    HttpMethod.Get,
                    "http://localhost:5147/weatherforecast"))
                {
                    requestMessage.Headers.Add("TRACE_ID", activity.Id);
                    
                    client.SendAsync(requestMessage).Wait();
                }
                
                // client.GetStringAsync("http://localhost:5147/weatherforecast").Wait();
            }
        }
    }

    static void InternalWorker()
        {
            using (var activity = MyActivitySource.StartActivity("InternalWorker"))
            {
                activity?.SetTag("foo", 1);
                activity?.SetTag("bar", "Hello, World!");
                activity?.SetTag("baz", new int[] {1, 2, 3});
                activity?.SetStatus(ActivityStatusCode.Ok);

                Thread.Sleep(100);
            }
        }
}


