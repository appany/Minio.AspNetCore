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