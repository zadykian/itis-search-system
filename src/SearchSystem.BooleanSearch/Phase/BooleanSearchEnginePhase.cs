using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.BooleanSearch.Phase
{
	// todo: implement
	
	/// <inheritdoc cref="IBooleanSearchEnginePhase" />
	internal class BooleanSearchEnginePhase : EnginePhaseBase<IDocumentsIndex, Unit>, IBooleanSearchEnginePhase
	{
		public BooleanSearchEnginePhase(
			IAppEnvironment<EnginePhaseBase<IDocumentsIndex, Unit>> appEnvironment) : base(appEnvironment)
		{
		}

		/// <inheritdoc />
		protected override Task<Unit> ExecuteAnewAsync(IDocumentsIndex inputData) => Task.FromResult(Unit.Instance);

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync() => Task.FromResult(Unit.Instance);
	}
}