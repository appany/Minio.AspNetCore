using Microsoft.Extensions.Options;

namespace Minio.AspNetCore
{
  public class MinioClientFactory : IMinioClientFactory
  {
    private readonly IOptionsMonitor<MinioOptions> optionsMonitor;

    public MinioClientFactory(IOptionsMonitor<MinioOptions> optionsMonitor)
    {
      this.optionsMonitor = optionsMonitor;
    }

    public MinioClient CreateClient()
    {
      return CreateClient(Options.DefaultName);
    }

    public MinioClient CreateClient(string name)
    {
      var options = optionsMonitor.Get(name);

      var client = new MinioClient()
        .WithEndpoint(options.Endpoint)
        .WithCredentials(options.AccessKey, options.SecretKey)
        .WithSessionToken(options.SessionToken);

      if (!string.IsNullOrEmpty(options.Region))
      {
        client.WithRegion(options.Region);
      }

      options.Configure?.Invoke(client);

      return client;
    }
  }
}
