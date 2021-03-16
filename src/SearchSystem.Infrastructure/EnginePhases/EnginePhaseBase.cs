using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.AppEnvironment;

namespace SearchSystem.Infrastructure.EnginePhases
{
	/// <inheritdoc />
	public abstract class EnginePhaseBase<TIn, TOut> : ISearchEnginePhase<TIn, TOut>
	{
		protected EnginePhaseBase(IAppEnvironment<EnginePhaseBase<TIn, TOut>> appEnvironment)
			=> Environment = appEnvironment;

		/// <inheritdoc cref="IAppEnvironment{T}"/>>
		protected IAppEnvironment<EnginePhaseBase<TIn, TOut>> Environment { get; }

		/// <summary>
		/// Name of component which this phase belongs to.
		/// </summary>
		protected string ComponentName => GetType().Name.Replace("EnginePhase", string.Empty);

		/// <inheritdoc />
		async Task<TOut> ISearchEnginePhase<TIn, TOut>.ExecuteAsync(TIn inputData)
		{
			Environment.Logger.LogInformation($"Phase '{ComponentName}' is started.");

			TOut output;
			try
			{
				output = Environment.Configuration.UsePreviousResultsFor(ComponentName)
					? await LoadPreviousResults()
					: await CreateNewData(inputData);
			}
			catch (Exception exception)
			{
				Environment.Logger.LogError("Error occured during search phase execution.", exception);
				throw;
			}

			Environment.Logger.LogInformation($"Phase '{ComponentName}' is finished successfully.");
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