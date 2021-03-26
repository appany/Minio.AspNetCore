namespace Minio.AspNetCore
{
  public interface IMinioClientFactory
  {
    MinioClient CreateClient();
    MinioClient CreateClient(string name);
  }
}
