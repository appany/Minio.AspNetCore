using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Minio.AspNetCore.HealthChecks
{
	public static class MinioHealthChecksExtensions
	{
		public static IHealthChecksBuilder AddMinio(
			this IHealthChecksBuilder builder,
			Func<IServiceProvider, MinioClient> factory,
			string name = "minio",
			HealthStatus? failureStatus = null,
			IEnumerable<string>? tags = null,
			TimeSpan? timeout = null)
		{
			return builder.Add(new HealthCheckRegistration(
				name,
				sp => new MinioHealthCheck(factory.Invoke(sp)),
				failureStatus,
				tags,
				timeout));
		}
	}
}