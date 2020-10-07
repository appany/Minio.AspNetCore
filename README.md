# Minio.AspNetCore

[![Nuget](https://img.shields.io/nuget/v/Minio.AspNetCore.svg)](https://www.nuget.org/packages/Minio.AspNetCore) ![Minio](https://github.com/appany/Minio.AspNetCore/workflows/Minio/badge.svg?branch=master)

`Microsoft.Extensions.DependencyInjection` extensions for [Minio](https://github.com/minio/minio-dotnet) client

## Usage

```cs
// Simple usage

services.AddMinio(options =>
{
  options.Endpoint = "endpoint";
  // ...
  options.OnClientConfiguration = client =>
  {
    client.WithSSL();
  }
});

// Get or inject
var client = serviceProvider.GetRequiredService<MinioClient>();

// Create new from factory
var client = serviceProvider.GetRequiredService<IMinioClientFactory>().CreateClient();
```

```cs
// Multiple clients support via named options

services.AddMinio(options =>
{
  options.Endpoint = "endpoint1";
  // ...
  options.OnClientConfiguration = client =>
  {
    client.WithSSL();
  }
});

// Via extension overload
services.AddMinio("minio2", options =>
{
  options.Endpoint = "endpoint2";
  // ...
  options.OnClientConfiguration = client =>
  {
    client.WithSSL().WithTimeout(...);
  }
});

// Via explicit named Configure
services.AddMinio()
  .Configure<MinioOptions>("minio3", options =>
  {
    options.Endpoint = "endpoint3";
    // ...
  });

// Get or inject first minio client
var client = serviceProvider.GetRequiredService<MinioClient>();

// Create new minio2
var client = serviceProvider.GetRequiredService<IMinioClientFactory>().CreateClient("minio2");

// Create new minio3
var client = serviceProvider.GetRequiredService<IMinioClientFactory>().CreateClient("minio3");
```
