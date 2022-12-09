using System;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcService;

namespace GrpcFrameworkClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectWithHttp2ToTestService();
        }

        private static void ConnectWithWebToTestService()
        {
            GrpcChannelOptions options4Web = new GrpcChannelOptions()
            {
                HttpHandler = new GrpcWebHandler(new HttpClientHandler())
            };

            using (GrpcChannel channel = GrpcChannel.ForAddress(
                       "https://localhost:7149",
                       options4Web))
            {
                Greeter.GreeterClient greeterClient = new Greeter.GreeterClient(channel);
                var response = greeterClient.SayHello(new HelloRequest());
                Console.WriteLine(response.Message);
            }
        }

        private static void ConnectWithHttp2ToTestService()
        {
            GrpcChannelOptions options4Http2 = new GrpcChannelOptions()
            {
                HttpHandler = new WinHttpHandler()
            };

            using (GrpcChannel channel = GrpcChannel.ForAddress(
                       "https://localhost:5001",
                       options4Http2))
            {
                Greeter.GreeterClient greeterClient = new Greeter.GreeterClient(channel);
                var response = greeterClient.SayHello(new HelloRequest());
                Console.WriteLine(response.Message);
            }
        }
    }
}
