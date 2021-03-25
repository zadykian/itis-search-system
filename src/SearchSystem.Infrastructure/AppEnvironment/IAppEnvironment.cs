using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents.Storage;

namespace SearchSystem.Infrastructure.AppEnvironment
{
	/// <summary>
	/// Application environment.
	/// </summary>
	public interface IAppEnvironment<out T>
	{
		/// <inheritdoc cref="IAppConfiguration"/>
		IAppConfiguration Configuration { get; }

		/// <inheritdoc cref="ILogger{TCategoryName}"/>
		ILogger<T> Logger { get; }

		/// <inheritdoc cref="IDocumentStorage"/>
		IDocumentStorage Storage { get; }
	}
}