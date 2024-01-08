namespace Minio.AspNetCore
{
  public class MinioOptions
  {
    public string Endpoint { get; set; } = default!;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string SessionToken { get; set; } = string.Empty;

    internal Action<IMinioClient>? Configure { get; private set; }

    public void ConfigureClient(Action<IMinioClient> configure)
    {
      Configure = configure;
    }
  }
}
