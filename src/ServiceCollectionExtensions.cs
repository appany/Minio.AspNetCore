using System;
using Microsoft.Extensions.DependencyInjection;

namespace Minio.AspNetCore
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMinio(this IServiceCollection services, Action<MinioOptions> options)
		{
			return services.Configure(options)
				.AddSingleton<IMinioClientFactory, MinioClientFactory>()
				.AddSingleton(sp => sp.GetRequiredService<IMinioClientFactory>().CreateClient());
		}
	}
}