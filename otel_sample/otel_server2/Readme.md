The necessary service files were created with the following commands:

```
protoc --plugin=protoc-gen-grpc="D:\tools\grpc-protoc\grpc_csharp_plugin.exe" --csharp_out="." --grpc_out="." --grpc_opt=no_client --proto_path="E:\ws\opentelemetry-proto\" E:\ws\opentelemetry-proto\opentelemetry\proto\collector\trace\v1\trace_service.proto
protoc --plugin=protoc-gen-grpc="D:\tools\grpc-protoc\grpc_csharp_plugin.exe" --csharp_out="." --grpc_out="." --grpc_opt=no_client --proto_path="E:\ws\opentelemetry-proto\" E:\ws\opentelemetry-proto\opentelemetry\proto\common\v1\common.proto
protoc --plugin=protoc-gen-grpc="D:\tools\grpc-protoc\grpc_csharp_plugin.exe" --csharp_out="." --grpc_out="." --grpc_opt=no_client --proto_path="E:\ws\opentelemetry-proto\" E:\ws\opentelemetry-proto\opentelemetry\proto\resource\v1\resource.proto
protoc --plugin=protoc-gen-grpc="D:\tools\grpc-protoc\grpc_csharp_plugin.exe" --csharp_out="." --grpc_out="." --grpc_opt=no_client --proto_path="e:\ws\opentelemetry-proto\" e:\ws\opentelemetry-proto\opentelemetry\proto\trace\v1\trace.proto
```