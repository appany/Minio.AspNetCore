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

    public IMinioClient CreateClient()
    {
      return CreateClient(Options.DefaultName);
    }

    public IMinioClient CreateClient(string name)
    {
      var options = optionsMonitor.Get(name);

#pragma warning disable IDISP001
#pragma warning disable CA2000
      var client = new MinioClient()
#pragma warning restore CA2000
        .WithEndpoint(options.Endpoint)
        .WithCredentials(options.AccessKey, options.SecretKey)
        .WithSessionToken(options.SessionToken);
#pragma warning restore IDISP001

      if (!string.IsNullOrEmpty(options.Region))
      {
        client.WithRegion(options.Region);
      }

      options.Configure?.Invoke(client);

      return client.Build();
    }
  }
}
