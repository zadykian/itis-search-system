using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;

namespace SearchSystem.Infrastructure.EnginePhases
{
	/// <inheritdoc />
	public abstract class EnginePhaseBase<TIn, TOut> : ISearchEnginePhase<TIn, TOut>
	{
		private readonly IAppConfiguration appConfiguration;
		private readonly ILogger logger;

		protected EnginePhaseBase(IAppConfiguration appConfiguration, ILogger logger)
		{
			this.appConfiguration = appConfiguration;
			this.logger = logger;
		}

		/// <inheritdoc />
		async Task<TOut> ISearchEnginePhase<TIn, TOut>.ExecuteAsync(TIn inputData)
		{
			logger.LogInformation($"Phase '{ComponentName}' is started.");

			var output = appConfiguration.UsePreviousResultsFor(ComponentName)
				? await LoadPreviousResults()
				: await CreateNewData(inputData);

			logger.LogInformation($"Phase '{ComponentName}' is finished successfully.");
			return output;
		}

		/// <summary>
		/// Name of component which this phase belongs to.
		/// </summary>
		private string ComponentName => GetType().Name.Replace("EnginePhase", string.Empty);

		/// <summary>
		/// Perform new execution.
		/// </summary>
		protected abstract Task<TOut> CreateNewData(TIn inputData);

		/// <summary>
		/// Load results retrieved during previous execution.
		/// </summary>
		protected abstract Task<TOut> LoadPreviousResults();
	}
}