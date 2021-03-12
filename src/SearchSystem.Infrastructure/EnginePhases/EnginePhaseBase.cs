using System.Threading.Tasks;
using SearchSystem.Infrastructure.Configuration;

namespace SearchSystem.Infrastructure.EnginePhases
{
	/// <inheritdoc />
	public abstract class EnginePhaseBase<TIn, TOut> : ISearchEnginePhase<TIn, TOut>
	{
		private readonly IAppConfiguration appConfiguration;

		protected EnginePhaseBase(IAppConfiguration appConfiguration) => this.appConfiguration = appConfiguration;

		/// <inheritdoc />
		Task<TOut> ISearchEnginePhase<TIn, TOut>.ExecuteAsync(TIn inputData)
			=> appConfiguration.UsePreviousResultsFor(ComponentName)
				? LoadPreviousResults()
				: CreateNewData(inputData);

		/// <summary>
		/// Name of component which this phase belongs to.
		/// </summary>
		protected abstract string ComponentName { get; }

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