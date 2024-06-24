using Microsoft.Extensions.Diagnostics.HealthChecks;
using Minio.DataModel.Args;

namespace Minio.AspNetCore.HealthChecks
{
  public class MinioHealthCheck : IHealthCheck
  {
    private readonly IMinioClient minioClient;
    private readonly string bucket;

    public MinioHealthCheck(IMinioClient minioClient, string bucket)
    {
      this.minioClient = minioClient;
      this.bucket = bucket;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
      HealthCheckContext context,
      CancellationToken cancellationToken = new())
    {
      try
      {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucket);
        await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);

        return HealthCheckResult.Healthy();
      }
      catch (Exception exception)
      {
        return HealthCheckResult.Unhealthy(exception: exception);
      }
    }
  }
}
