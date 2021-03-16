using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.EnginePhases;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc cref="IBooleanSearchEnginePhase" />
	internal class BooleanSearchEnginePhase : EnginePhaseBase<IDocumentsIndex, Unit>, IBooleanSearchEnginePhase
	{
		public BooleanSearchEnginePhase(
			IAppEnvironment<EnginePhaseBase<IDocumentsIndex, Unit>> appEnvironment) : base(appEnvironment)
		{
		}

		/// <inheritdoc />
		protected override Task<Unit> CreateNewData(IDocumentsIndex inputData) => throw new System.NotImplementedException();

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResults() => Task.FromResult(Unit.Instance);
	}
}