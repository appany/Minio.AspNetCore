# Minio.AspNetCore

[![License](https://img.shields.io/github/license/appany/Minio.AspNetCore.svg)](https://github.com/appany/Minio.AspnetCore/blob/main/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/Minio.AspNetCore.svg)](https://www.nuget.org/packages/Minio.AspNetCore)
[![Downloads](https://img.shields.io/nuget/dt/Minio.AspNetCore)](https://www.nuget.org/packages/Minio.AspNetCore)
![Tests](https://github.com/appany/Minio.AspNetCore/workflows/Tests/badge.svg)
[![Coverage Status](https://coveralls.io/repos/github/appany/Minio.AspNetCore/badge.svg?branch=main)](https://coveralls.io/github/appany/Minio.AspNetCore?branch=main)

`Microsoft.Extensions.DependencyInjection` and `HealthChecks` extensions for [Minio](https://github.com/minio/minio-dotnet) client

## Installation

```bash
$> dotnet add package Minio.AspNetCore
```

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
