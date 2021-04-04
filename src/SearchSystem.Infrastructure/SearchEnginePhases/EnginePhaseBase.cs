using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.AppEnvironment;

namespace SearchSystem.Infrastructure.SearchEnginePhases
{
	/// <inheritdoc />
	public abstract class EnginePhaseBase<TIn, TOut> : ISearchEnginePhase<TIn, TOut>
	{
		protected EnginePhaseBase(IAppEnvironment<EnginePhaseBase<TIn, TOut>> appEnvironment)
			=> AppEnvironment = appEnvironment;

		/// <inheritdoc cref="IAppEnvironment{T}"/>>
		protected IAppEnvironment<EnginePhaseBase<TIn, TOut>> AppEnvironment { get; }

		/// <summary>
		/// Name of component which this phase belongs to.
		/// </summary>
		protected string ComponentName 
			=> GetType()
				.Name
				.Replace("EnginePhase", string.Empty)
				.Replace("Subphase", string.Empty);

		/// <inheritdoc />
		async Task<TOut> ISearchEnginePhase<TIn, TOut>.ExecuteAsync(TIn inputData)
		{
			AppEnvironment.Logger.LogInformation($"Phase '{ComponentName}' is started.");

			TOut output;
			try
			{
				output = AppEnvironment.Configuration.UsePreviousResultsFor(ComponentName)
					? await LoadPreviousResultsAsync()
					: await ExecuteAnewAsync(inputData);
			}
			catch (Exception exception)
			{
				AppEnvironment.Logger.LogError(exception, "Error occured during search phase execution.");
				throw;
			}

			AppEnvironment.Logger.LogInformation($"Phase '{ComponentName}' is finished successfully.");
			return output;
		}

		/// <summary>
		/// Perform new execution.
		/// </summary>
		protected abstract Task<TOut> ExecuteAnewAsync(TIn inputData);

		/// <summary>
		/// Load results retrieved during previous execution.
		/// </summary>
		protected abstract Task<TOut> LoadPreviousResultsAsync();
	}
}