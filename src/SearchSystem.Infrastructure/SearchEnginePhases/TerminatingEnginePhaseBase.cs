using System.Threading.Tasks;
using SearchSystem.Infrastructure.AppEnvironment;

namespace SearchSystem.Infrastructure.SearchEnginePhases
{
	/// <summary>
	/// Base class for phases which does not produce any output. 
	/// </summary>
	public abstract class TerminatingEnginePhaseBase<TIn> : EnginePhaseBase<TIn, Unit>
	{
		protected TerminatingEnginePhaseBase(IAppEnvironment<TerminatingEnginePhaseBase<TIn>> appEnvironment) : base(appEnvironment)
		{
		}

		/// <inheritdoc />
		protected sealed override async Task<Unit> ExecuteAnewAsync(TIn inputData)
		{
			await ExecuteAsync(inputData);
			return Unit.Instance;
		}

		/// <summary>
		/// Perform execution based on <paramref name="inputData"/>.
		/// </summary>
		protected abstract Task ExecuteAsync(TIn inputData);

		/// <inheritdoc />
		protected sealed override Task<Unit> LoadPreviousResultsAsync() => Task.FromResult(Unit.Instance);
	}
}