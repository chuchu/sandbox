using Grpc.Core;
using grpc_otel;

namespace grpc_otel.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name + " " + CallWeatherForecast()
        });
    }

    private string CallWeatherForecast()
    {
        var client = new HttpClient();
        return client.GetStringAsync("http://localhost:5147/weatherforecast2").Result;
    }
}
