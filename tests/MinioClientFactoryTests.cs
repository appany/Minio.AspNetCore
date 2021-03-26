using System.Net;
using Microsoft.Extensions.Options;
using Xunit;

namespace Minio.AspNetCore.Tests
{
  public class MinioClientFactoryTests
  {
    private readonly IWebProxy webProxy;
    private readonly int timeout;

    public MinioClientFactoryTests()
    {
      webProxy = new WebProxy();
      timeout = 10;
    }

    [Fact]
    public void PassedOptions_Corrent()
    {
      var monitor = MinioOptionsTestHelper.CreateOptionsMonitor(
        new[]
        {
          new ConfigureOptions<MinioOptions>(options =>
          {
            options.Endpoint = "endpoint";
            options.Region = "region";
            options.AccessKey = "accesskey";
            options.SecretKey = "secretkey";
            options.SessionToken = "sessiontoken";
            options.OnClientConfiguration = minioClient =>
            {
              minioClient.WithSSL()
                .WithTimeout(timeout)
                .WithProxy(webProxy);
            };
          })
        });

      var factory = new MinioClientFactory(monitor);

      var client = factory.CreateClient();

      MinioAsserts.AssertOptionsMatch(client, monitor.Get(Options.DefaultName));
      MinioAsserts.AssertSecure(client);
      MinioAsserts.AssertTimeout(client, timeout);
      MinioAsserts.AssertWebProxy(client, webProxy);
    }

    [Fact]
    public void PassedOptions_Named_Corrent()
    {
      var monitor = MinioOptionsTestHelper.CreateOptionsMonitor(
        new IConfigureOptions<MinioOptions>[]
        {
          new ConfigureOptions<MinioOptions>(options =>
          {
            options.Endpoint = "endpoint1";
            options.Region = "region1";
            options.AccessKey = "accesskey1";
            options.SecretKey = "secretkey1";
            options.SessionToken = "sessiontoken1";
          }),
          new ConfigureNamedOptions<MinioOptions>(MinioOptionsTestHelper.CustomOptionsName, options =>
          {
            options.Endpoint = "endpoint2";
            options.Region = "region2";
            options.AccessKey = "accesskey2";
            options.SecretKey = "secretkey2";
            options.SessionToken = "sessiontoken2";
          }),
        });

      var factory = new MinioClientFactory(monitor);

      var client1 = factory.CreateClient(Options.DefaultName);
      MinioAsserts.AssertOptionsMatch(client1, monitor.Get(Options.DefaultName));

      var client2 = factory.CreateClient(MinioOptionsTestHelper.CustomOptionsName);
      MinioAsserts.AssertOptionsMatch(client2, monitor.Get(MinioOptionsTestHelper.CustomOptionsName));
    }
  }
}
