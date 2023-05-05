using System.Net;
using System.Reflection;
using Xunit;

namespace Minio.AspNetCore.Tests
{
  public static class MinioAsserts
  {
    public static void AssertOptionsMatch(MinioClient client, MinioOptions options)
    {
      if (client is null)
      {
        throw new ArgumentNullException(nameof(client));
      }

      if (options is null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      var clientType = client.GetType();

      var endpoint = clientType
        .GetProperty("BaseUrl", BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(client);
      Assert.Equal(options.Endpoint, endpoint);

      var region = clientType
        .GetField(nameof(MinioOptions.Region), BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(client);
      Assert.Equal(options.Region, region);

      var accessKey = clientType
        .GetProperty(nameof(MinioOptions.AccessKey), BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(client);
      Assert.Equal(options.AccessKey, accessKey);

      var secretKey = clientType
        .GetProperty(nameof(MinioOptions.SecretKey), BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(client);
      Assert.Equal(options.SecretKey, secretKey);

      var sessionToken = clientType
        .GetProperty(nameof(MinioOptions.SessionToken), BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(client);
      Assert.Equal(options.SessionToken, sessionToken);
    }

    public static void AssertSecure(MinioClient client)
    {
      if (client is null)
      {
        throw new ArgumentNullException(nameof(client));
      }

      var clientType = client.GetType();

      var secure = (bool)clientType
        .GetProperty("Secure", BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(client)!;

      Assert.True(secure);
    }

    public static void AssertTimeout(MinioClient client, int timeout)
    {
      if (client is null)
      {
        throw new ArgumentNullException(nameof(client));
      }

      var clientType = client.GetType();

      var clientTimeout = (int)clientType
        .GetField("RequestTimeout", BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(client)!;

      Assert.Equal(timeout, clientTimeout);
    }

    public static void AssertWebProxy(MinioClient client, IWebProxy webProxy)
    {
      if (client is null)
      {
        throw new ArgumentNullException(nameof(client));
      }

      var clientType = client.GetType();

      var clientProxy = clientType
        .GetProperty("Proxy", BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(client) as IWebProxy;

      Assert.Equal(webProxy, clientProxy);
    }
  }
}
