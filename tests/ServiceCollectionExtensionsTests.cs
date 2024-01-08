using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Minio.AspNetCore.Tests
{
  public class ServiceCollectionExtensionsTests
  {
    private readonly IServiceCollection services;

    public ServiceCollectionExtensionsTests()
    {
      services = new ServiceCollection()
        .AddMinio(options =>
        {
          options.Endpoint = "endpoint";
          options.Region = "region";
          options.AccessKey = "accesskey";
          options.SecretKey = "secretkey";
          options.SessionToken = "sessiontoken";
        });
    }

    [Fact]
    public void AddToServices()
    {
      Assert.Contains(services, x => x.ServiceType == typeof(IMinioClientFactory));
      Assert.Contains(services, x => x.ServiceType == typeof(IConfigureOptions<MinioOptions>));
      Assert.Contains(services, x => x.ServiceType == typeof(IMinioClient));
    }

    [Fact]
    public void GetFromServices()
    {
      using var serviceProvider = services.BuildServiceProvider();

      var factory = serviceProvider.GetService<IMinioClientFactory>();
      Assert.NotNull(factory);

      var client = serviceProvider.GetService<IMinioClient>();
      Assert.NotNull(client);

      var options = serviceProvider.GetService<IOptions<MinioOptions>>()?.Value;
      Assert.NotNull(options);

      Assert.Equal("endpoint", options.Endpoint);
      Assert.Equal("region", options.Region);
      Assert.Equal("accesskey", options.AccessKey);
      Assert.Equal("secretkey", options.SecretKey);
      Assert.Equal("sessiontoken", options.SessionToken);

      MinioAsserts.AssertOptionsMatch(client, options);
    }

    [Fact]
    public void SameClients()
    {
      using var serviceProvider = services.BuildServiceProvider();

      var client1 = serviceProvider.GetRequiredService<IMinioClient>();
      var client2 = serviceProvider.GetRequiredService<IMinioClient>();

      Assert.Same(client1, client2);
    }

    [Fact]
    public void MultipleClients()
    {
      using var serviceProvider = services.BuildServiceProvider();

      var client1 = serviceProvider.GetRequiredService<IMinioClient>();
      using var client2 = serviceProvider.GetRequiredService<IMinioClientFactory>().CreateClient();

      Assert.NotSame(client1, client2);
    }
  }
}
