# Minio.AspNetCore

[![Nuget](https://img.shields.io/nuget/v/Minio.AspNetCore.svg)](https://www.nuget.org/packages/Minio.AspNetCore) ![Minio](https://github.com/appany/Minio.AspNetCore/workflows/Minio/badge.svg?branch=master)

`Microsoft.Extensions.DependencyInjection` and `HealthChecks` extensions for [Minio](https://github.com/minio/minio-dotnet) client

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
});

// Url based configuration
services.AddMinio(new Uri("s3://accessKey:secretKey@localhost:9000/region"));

// Get or inject
var client = serviceProvider.GetRequiredService<MinioClient>();

// Create new from factory
var client = serviceProvider.GetRequiredService<IMinioClientFactory>().CreateClient();
```

## Multiple clients support using named options

```cs
services.AddMinio(options =>
{
  options.Endpoint = "endpoint1";
  // ...
  options.OnClientConfiguration = client =>
  {
    client.WithSSL();
  }
});

// Named extension overload
services.AddMinio("minio2", options =>
{
  options.Endpoint = "endpoint2";
  // ...
  options.OnClientConfiguration = client =>
  {
    client.WithSSL().WithTimeout(...);
  }
});

// Explicit named Configure
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

## HealthChecks

```cs
// Minio.AspNetCore.HealthChecks package

services.AddHealthChecks()
  .AddMinio(sp => sp.GetRequiredService<MinioClient>());

services.AddHealthChecks()
  .AddMinio(sp => sp.GetRequiredService<MinioClient>())
  .AddMinio(sp => /* Get named client from cache or create new */);
```
