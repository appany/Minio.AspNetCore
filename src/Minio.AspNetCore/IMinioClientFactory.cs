namespace Minio.AspNetCore
{
  public interface IMinioClientFactory
  {
    IMinioClient CreateClient();
    IMinioClient CreateClient(string name);
  }
}
