using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Minio.AspNetCore.Tests
{
  public class ServiceCollectionUrlExtensionsTests
  {
    [Fact]
    public void UrlBasedConfiguration()
    {
      var services = new ServiceCollection()
        .AddMinio(new Uri("s3://accessKey:secretKey@localhost:9000/region"));

      var serviceProvider = services.BuildServiceProvider();

      var options = serviceProvider.GetRequiredService<IOptions<MinioOptions>>().Value;

      Assert.Equal("accessKey", options.AccessKey);
      Assert.Equal("secretKey", options.SecretKey);
      Assert.Equal("localhost:9000", options.Endpoint);
      Assert.Equal("region", options.Region);
    }

    [Fact]
    public void UrlBasedConfiguration_Endpoint_Overriden()
    {
      var services = new ServiceCollection()
        .AddMinio(new Uri("s3://accessKey:secretKey@localhost:9000/region"), o =>
        {
          o.Endpoint = "endpoint";
        });

      var serviceProvider = services.BuildServiceProvider();

      var options = serviceProvider.GetRequiredService<IOptions<MinioOptions>>().Value;

      Assert.Equal("accessKey", options.AccessKey);
      Assert.Equal("secretKey", options.SecretKey);
      Assert.Equal("endpoint", options.Endpoint);
      Assert.Equal("region", options.Region);
    }

    [Theory]
    [InlineData("accessKey:secretKey@localhost:9000/region")]
    [InlineData("accessKey:secretKey@localhost:9000")]
    [InlineData("s3://localhost:9000/region")]
    [InlineData("s3://localhost:9000")]
    [InlineData("localhost:9000")]
    public void UrlBasedConfiguration_InvalidCredentials_Throw(string url)
    {
      var services = new ServiceCollection()
        .AddMinio(new Uri(url));

      var serviceProvider = services.BuildServiceProvider();

      Assert.Throws<InvalidOperationException>(
        () => serviceProvider.GetRequiredService<IOptions<MinioOptions>>().Value);
    }
  }
}
