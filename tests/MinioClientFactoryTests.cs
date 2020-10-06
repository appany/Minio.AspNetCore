using System.Net;
using Microsoft.Extensions.Options;
using Xunit;

namespace Minio.AspNetCore.Tests
{
	public class MinioClientFactoryTests
	{
		private readonly IWebProxy webProxy;
		private readonly int timeout;

		public MinioClientFactoryTests()
		{
			webProxy = new WebProxy();
			timeout = 10;
		}

		[Fact]
		public void PassedOptions_Corrent()
		{
			var options = Options.Create(new MinioOptions
			{
				Endpoint = "endpoint",
				Region = "region",
				AccessKey = "accesskey",
				SecretKey = "secretkey",
				SessionToken = "sessiontoken",
				OnClientConfiguration = minioClient =>
				{
					minioClient.WithSSL()
						.WithTimeout(timeout)
						.WithProxy(webProxy);
				}
			});

			var factory = new MinioClientFactory(options);

			var client = factory.CreateClient();

			MinioAsserts.AssertOptionsMatch(client, options.Value);
			MinioAsserts.AssertSecure(client);
			MinioAsserts.AssertTimeout(client, timeout);
			MinioAsserts.AssertWebProxy(client, webProxy);
		}
	}
}