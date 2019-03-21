set BINDIR=%USERPROFILE%\.nuget\packages\google.protobuf.tools\3.7.0\tools\windows_x64

%BINDIR%\protoc.exe --proto_path=. -I=protolib --csharp_out=. rpc.proto

pause