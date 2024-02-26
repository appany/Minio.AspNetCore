using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Minio.AspNetCore.HealthChecks
{
  public class MinioHealthCheck : IHealthCheck
  {
    private readonly IMinioClient _minioClient;

    public MinioHealthCheck(IMinioClient minioClient)
    {
      _minioClient = minioClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
      HealthCheckContext context,
      CancellationToken cancellationToken = new())
    {
      try
      {
        await _minioClient.ListBucketsAsync(cancellationToken).ConfigureAwait(false);

        return HealthCheckResult.Healthy();
      }
      catch (Exception exception)
      {
        return HealthCheckResult.Unhealthy(exception: exception);
      }
    }
  }
}
