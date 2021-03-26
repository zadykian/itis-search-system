using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents.Storage;

namespace SearchSystem.Infrastructure.AppEnvironment
{
	/// <inheritdoc />
	internal class DefaultAppEnvironment<T> : IAppEnvironment<T>
	{
		public DefaultAppEnvironment(
			IAppConfiguration configuration,
			ILogger<T> logger,
			IDocumentStorage storage)
		{
			Configuration = configuration;
			Logger = logger;
			Storage = storage;
		}

		/// <inheritdoc />
		public IAppConfiguration Configuration { get; }

		/// <inheritdoc />
		public ILogger<T> Logger { get; }

		/// <inheritdoc />
		public IDocumentStorage Storage { get; }
	}
}