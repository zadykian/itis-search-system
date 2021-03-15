using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;

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
	}
}