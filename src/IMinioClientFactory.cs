namespace Minio.AspNetCore
{
	public interface IMinioClientFactory
	{
		MinioClient CreateClient();
	}
}