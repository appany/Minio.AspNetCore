using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Minio.AspNetCore.HealthChecks
{
  public static class MinioHealthChecksExtensions
  {
    public static IHealthChecksBuilder AddMinio(
      this IHealthChecksBuilder builder,
      Func<IServiceProvider, IMinioClient> factory,
      string name = "minio",
      HealthStatus? failureStatus = null,
      IEnumerable<string>? tags = null,
      TimeSpan? timeout = null)
    {
      if (builder is null)
      {
        throw new ArgumentNullException(nameof(builder));
      }

      return builder.Add(new HealthCheckRegistration(
        name,
        sp => new MinioHealthCheck(factory.Invoke(sp)),
        failureStatus,
        tags,
        timeout));
    }
  }
}
