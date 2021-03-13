using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;

namespace SearchSystem.Infrastructure.EnginePhases
{
	/// <inheritdoc />
	public abstract class EnginePhaseBase<TIn, TOut> : ISearchEnginePhase<TIn, TOut>
	{
		private readonly IAppConfiguration appConfiguration;

		protected EnginePhaseBase(IAppConfiguration appConfiguration, ILogger<EnginePhaseBase<TIn, TOut>> logger)
		{
			this.appConfiguration = appConfiguration;
			Logger = logger;
		}

		/// <summary>
		/// Logger.
		/// </summary>
		protected ILogger<EnginePhaseBase<TIn, TOut>> Logger { get; }

		/// <summary>
		/// Name of component which this phase belongs to.
		/// </summary>
		protected string ComponentName => GetType().Name.Replace("EnginePhase", string.Empty);

		/// <inheritdoc />
		async Task<TOut> ISearchEnginePhase<TIn, TOut>.ExecuteAsync(TIn inputData)
		{
			Logger.LogInformation($"Phase '{ComponentName}' is started.");

			var output = appConfiguration.UsePreviousResultsFor(ComponentName)
				? await LoadPreviousResults()
				: await CreateNewData(inputData);

			Logger.LogInformation($"Phase '{ComponentName}' is finished successfully.");
			return output;
		}

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