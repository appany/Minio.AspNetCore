using System.Net;
using System.Reflection;
using Xunit;

namespace Minio.AspNetCore.Tests
{
  public static class MinioAsserts
  {
    public static void AssertOptionsMatch(IMinioClient client, MinioOptions options)
    {
      ArgumentNullException.ThrowIfNull(client);
      ArgumentNullException.ThrowIfNull(options);

      Assert.Equal($"{(client.Config.Secure ? "https" : "http")}://{options.Endpoint}", client.Config.Endpoint);
      Assert.Equal(options.Region, client.Config.Region);
      Assert.Equal(options.AccessKey, client.Config.AccessKey);
      Assert.Equal(options.SecretKey, client.Config.SecretKey);
      Assert.Equal(options.SessionToken, client.Config.SessionToken);
    }

    public static void AssertSecure(IMinioClient client)
    {
      ArgumentNullException.ThrowIfNull(client);

      Assert.True(client.Config.Secure);
    }

    public static void AssertTimeout(IMinioClient client, int timeout)
    {
      ArgumentNullException.ThrowIfNull(client);

      Assert.Equal(timeout, client.Config.RequestTimeout);
    }

    public static void AssertWebProxy(IMinioClient client, IWebProxy webProxy)
    {
      ArgumentNullException.ThrowIfNull(client);

      Assert.Equal(webProxy, client.Config.Proxy);
    }
  }
}
