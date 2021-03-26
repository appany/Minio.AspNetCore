using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Minio.AspNetCore.HealthChecks
{
  public class MinioHealthCheck : IHealthCheck
  {
    private readonly MinioClient minioClient;

    public MinioHealthCheck(MinioClient minioClient)
    {
      this.minioClient = minioClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
      HealthCheckContext context,
      CancellationToken cancellationToken = new())
    {
      try
      {
        await minioClient.ListBucketsAsync(cancellationToken);

        return HealthCheckResult.Healthy();
      }
      catch (Exception exception)
      {
        return HealthCheckResult.Unhealthy(exception: exception);
      }
    }
  }
}
