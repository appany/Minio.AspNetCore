using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Minio.AspNetCore.Tests
{
  public class ServiceCollectionNamedExtensionsTests
  {
    private readonly IServiceCollection services;

    public ServiceCollectionNamedExtensionsTests()
    {
      services = new ServiceCollection()
        .AddMinio(MinioOptionsTestHelper.CustomOptionsName, options =>
        {
          options.Endpoint = "endpoint";
          options.Region = "region";
          options.AccessKey = "accesskey";
          options.SecretKey = "secretkey";
          options.SessionToken = "sessiontoken";
        });
    }

    [Fact]
    public void GetFromServices()
    {
      var serviceProvider = services.BuildServiceProvider();

      var factory = serviceProvider.GetService<IMinioClientFactory>();
      Assert.NotNull(factory);

      var client = serviceProvider.GetService<MinioClient>();
      Assert.NotNull(client);

      var monitor = serviceProvider.GetRequiredService<IOptionsMonitor<MinioOptions>>();

      var options = monitor.Get(MinioOptionsTestHelper.CustomOptionsName);

      Assert.Equal("endpoint", options.Endpoint);
      Assert.Equal("region", options.Region);
      Assert.Equal("accesskey", options.AccessKey);
      Assert.Equal("secretkey", options.SecretKey);
      Assert.Equal("sessiontoken", options.SessionToken);

      MinioAsserts.AssertOptionsMatch(client!, options);
    }
  }
}
