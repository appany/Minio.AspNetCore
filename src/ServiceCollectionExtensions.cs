using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Minio.AspNetCore
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMinio(this IServiceCollection services)
		{
			return services.AddMinio(options => {});
		}

		public static IServiceCollection AddMinio(this IServiceCollection services, Action<MinioOptions> options)
		{
			return services.AddMinio(Options.DefaultName, options);
		}

		public static IServiceCollection AddMinio(this IServiceCollection services, string name, Action<MinioOptions> options)
		{
			services.Configure(name, options);
			services.TryAddSingleton<IMinioClientFactory, MinioClientFactory>();
			services.TryAddSingleton(sp => sp.GetRequiredService<IMinioClientFactory>().CreateClient(name));

			return services;
		}
	}
}