tools\gRPC\1.2.2\protoc.exe -Isrc\CoreCover.Instrumentation --csharp_out src\CoreCover.Instrumentation\ --grpc_out src\CoreCover.Instrumentation\ src\CoreCover.Instrumentation\ExecutionTracker.proto --plugin=protoc-gen-grpc=tools\gRPC\1.2.2\grpc_csharp_plugin.exe