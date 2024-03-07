using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Minio.AspNetCore.HealthChecks
{
  public static class MinioHealthChecksExtensions
  {
    /// <summary>
    /// Adds a health check for MinIO to the specified <see cref="builder"/>.
    /// The health check tests if a bucket with the specified <see cref="bucket"/> name exists.
    /// Whether the bucket actually exists doesn't matter - the health check is successful if the call to MinIO doesn't throw.
    /// </summary>
    public static IHealthChecksBuilder AddMinio(
      this IHealthChecksBuilder builder,
      Func<IServiceProvider, MinioClient> factory,
      string name = "minio",
      string bucket = "health",
      HealthStatus? failureStatus = null,
      IEnumerable<string>? tags = null,
      TimeSpan? timeout = null)
    {
      if (builder is null)
      {
        throw new ArgumentNullException(nameof(builder));
      }
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentException("Health check name should not be null or whitespace.", nameof(name));
      }
      if (string.IsNullOrWhiteSpace(bucket))
      {
        throw new ArgumentException("Health check bucket should not be null or whitespace.", nameof(bucket));
      }

      return builder.Add(new HealthCheckRegistration(
        name,
        sp => new MinioHealthCheck(factory.Invoke(sp), bucket),
        failureStatus,
        tags,
        timeout));
    }
  }
}
