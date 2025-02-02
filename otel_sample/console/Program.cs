using System.Diagnostics;
using library;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Grpc.Net.Client;
using grpc_otel;
using System.Text;

internal class Program
{
    private static readonly ActivitySource GlobalActivitySource =
        new ActivitySource("OTELSample.Console");

    private static void Main(string[] args)
    {
        Console.WriteLine("Starting application ...");

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService(
                serviceName: "OTELSampleConsole",
                serviceVersion: "1.0.0"))
            .AddSource("OTELSample.Console", "OTELSample.Library")
            .AddGrpcClientInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter()
            .Build();

        // Here the std out of the current process can be captured.
        // This is useful for testing purposes.
        TextWriter originalOut = Console.Out;
        using MemoryStream ms = new MemoryStream();
        using StreamWriter sw = new StreamWriter(ms);
        Console.SetOut(sw);

        using var activity = 
          GlobalActivitySource.StartActivity(
              "ConsoleWorker");
        
        activity?.SetTag("foo", 1);
        activity?.SetTag("bar", "Hello, World!");
        activity?.SetTag("baz", new int[] { 1, 2, 3 });

        Thread.Sleep(100);
        CallLibWorker();
        Thread.Sleep(200);

        CallInternalWorker();

        //CallWeatherForecast();

        //CallGreeterService().Wait();

        activity?.SetStatus(ActivityStatusCode.Ok);

        Console.SetOut(originalOut);
        ms.Seek(0, SeekOrigin.Begin);
        StreamReader sr = new StreamReader(ms);
        Console.WriteLine("Captured output:");
        Console.WriteLine(sr.ReadToEnd());
    }

    static void CallLibWorker()
    {
        using var activity = GlobalActivitySource.StartActivity("LibWorker");
        Worker w = new Worker();
        w.Work();
    }

    static void CallWeatherForecast()
    {
        using var activity = GlobalActivitySource.StartActivity("WeatherForecast");
        var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "http://localhost:5147/weatherforecast");
        requestMessage.Headers.Add("TRACE_ID", activity?.Id);
        client.SendAsync(requestMessage).Wait();
        
        client.GetStringAsync("http://localhost:5147/weatherforecast2").Wait();
    }

    static void CallInternalWorker()
    {
        using var activity = GlobalActivitySource.StartActivity("InternalWorker");
        activity?.SetTag("foo", 1);
        activity?.SetTag("bar", "Hello, World!");
        activity?.SetTag("baz", new int[] { 1, 2, 3 });
        Thread.Sleep(100);
        activity?.SetStatus(ActivityStatusCode.Ok);
    }

    static async Task CallGreeterService()
    {
        using var activity = GlobalActivitySource.StartActivity("GreeterService");
        using var channel = GrpcChannel.ForAddress("http://localhost:5267");
        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(
            new HelloRequest
            {
                Name = "GreeterClient"
            });
        Console.WriteLine("Greeting: " + reply.Message);
    }
}
