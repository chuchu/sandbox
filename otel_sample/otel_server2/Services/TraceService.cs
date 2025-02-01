using Grpc.Core;
using OpenTelemetry.Proto.Collector.Trace.V1;
using OpenTelemetry.Proto.Trace.V1;

namespace OtelServer2
{
  public class TraceService : OpenTelemetry.Proto.Collector.Trace.V1.TraceService.TraceServiceBase
  {

      public override Task<ExportTraceServiceResponse> Export(
        ExportTraceServiceRequest request,
        ServerCallContext context)
      {
        foreach(ResourceSpans resourceSpans in request.ResourceSpans)
        {
            foreach(ScopeSpans scopeSpans in resourceSpans.ScopeSpans)
            {
                foreach (Span span in scopeSpans.Spans)
                {
                    Console.WriteLine("Span: " + span);
                }
            }
        }
        
        return Task.FromResult(new ExportTraceServiceResponse());
      }
  }
}
