using Microsoft.Extensions.Options;

namespace Minio.AspNetCore
{
	public class MinioClientFactory : IMinioClientFactory
	{
		private readonly MinioOptions options;

		public MinioClientFactory(IOptions<MinioOptions> options)
		{
			this.options = options.Value;
		}

		public MinioClient CreateClient()
		{
			var client = new MinioClient(
				options.Endpoint,
				options.AccessKey,
				options.SecretKey,
				options.Region,
				options.SessionToken);

			options.OnClientConfiguration?.Invoke(client);

			return client;
		}
	}
}