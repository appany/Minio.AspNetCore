using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Minio.AspNetCore.Tests
{
	public static class MinioOptionsTestHelper
	{
		public const string CustomOptionsName = "custom";

		public static OptionsMonitor<MinioOptions> CreateOptionsMonitor(
			IEnumerable<IConfigureOptions<MinioOptions>> configureOptions)
		{
			return new OptionsMonitor<MinioOptions>(
				new OptionsFactory<MinioOptions>(
					configureOptions,
					Array.Empty<IPostConfigureOptions<MinioOptions>>(),
					Array.Empty<IValidateOptions<MinioOptions>>()),
				Array.Empty<IOptionsChangeTokenSource<MinioOptions>>(),
				new OptionsCache<MinioOptions>());
		}
	}
}