using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Minio.AspNetCore.Tests
{
	public class ServiceCollectionExtensionsTests
	{
		private readonly IServiceCollection services;

		public ServiceCollectionExtensionsTests()
		{
			services = new ServiceCollection()
				.AddMinio(options =>
				{
					options.Endpoint = "endpoint";
					options.Region = "region";
					options.AccessKey = "accesskey";
					options.SecretKey = "secretkey";
					options.SessionToken = "sessiontoken";
				});
		}

		[Fact]
		public void AddToServices()
		{
			Assert.True(services.Any(x => x.ServiceType == typeof(IMinioClientFactory)));
			Assert.True(services.Any(x => x.ServiceType == typeof(IConfigureOptions<MinioOptions>)));
			Assert.True(services.Any(x => x.ServiceType == typeof(MinioClient)));
		}

		[Fact]
		public void GetFromServices()
		{
			var serviceProvider = services.BuildServiceProvider();

			var factory = serviceProvider.GetService<IMinioClientFactory>();
			Assert.NotNull(factory);

			var client = serviceProvider.GetService<MinioClient>();
			Assert.NotNull(client);

			var options = serviceProvider.GetService<IOptions<MinioOptions>>()?.Value;
			Assert.NotNull(options);

			Assert.Equal("endpoint", options!.Endpoint);
			Assert.Equal("region", options.Region);
			Assert.Equal("accesskey", options.AccessKey);
			Assert.Equal("secretkey", options.SecretKey);
			Assert.Equal("sessiontoken", options.SessionToken);

			MinioAsserts.AssertOptionsMatch(client!, options);
		}

		[Fact]
		public void SameClients()
		{
			var serviceProvider = services.BuildServiceProvider();

			var client1 = serviceProvider.GetRequiredService<MinioClient>();
			var client2 = serviceProvider.GetRequiredService<MinioClient>();

			Assert.Same(client1, client2);
		}

		[Fact]
		public void MultipleClients()
		{
			var serviceProvider = services.BuildServiceProvider();

			var client1 = serviceProvider.GetRequiredService<MinioClient>();
			var client2 = serviceProvider.GetRequiredService<IMinioClientFactory>().CreateClient();

			Assert.NotSame(client1, client2);
		}
	}
}