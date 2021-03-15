using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;

namespace SearchSystem.Infrastructure.AppEnvironment
{
	/// <inheritdoc />
	internal class DefaultAppEnvironment<T> : IAppEnvironment<T>
	{
		public DefaultAppEnvironment(
			IAppConfiguration configuration,
			ILogger<T> logger)
		{
			Configuration = configuration;
			Logger = logger;
		}

		/// <inheritdoc />
		public IAppConfiguration Configuration { get; }

		/// <inheritdoc />
		public ILogger<T> Logger { get; }
	}
}