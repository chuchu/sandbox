using System.Diagnostics;

using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Exporter;

namespace Tools
{
    class Worker
    {
        public void Work()
        {
            // Define some important constants to initialize tracing with
            var serviceName = "MyCompany.MyProduct.MyService";
            var serviceVersion = "1.0.0";

            // Configure important OpenTelemetry settings and the console exporter
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(serviceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                .AddConsoleExporter()
                .AddOtlpExporter(opt =>
                {
                    opt.Protocol = OtlpExportProtocol.Grpc;
                })
                .Build();

            var MyActivitySource = new ActivitySource(serviceName);

            using var activity = MyActivitySource.StartActivity("WorkerWork");
            activity?.SetTag("foo", 1);
            activity?.SetTag("bar", "Hello, World!");
            activity?.SetTag("baz", new int[] { 1, 2, 3 });

            Thread.Sleep(500);
        }
    }
}