# Minio.AspNetCore

[![Nuget](https://img.shields.io/nuget/v/Minio.AspNetCore.svg)](https://www.nuget.org/packages/Minio.AspNetCore) ![Minio](https://github.com/appany/Minio.AspNetCore/workflows/Minio/badge.svg?branch=master)

`Microsoft.Extensions.DependencyInjection` extensions for [Minio](https://github.com/minio/minio-dotnet) client

## Usage

```cs
services.AddMinio(options =>
{
  options.Endpoint = "endpoint";
  // ...
  options.OnClientConfiguration = client =>
  {
    client.WithSSL();
  }
})

// Get or inject
var client = serviceProvider.GetRequiredService<MinioClient>();

// Create new from factory
var client = serviceProvider.GetRequiredService<IMinioClientFactory>().CreateClient();
```
