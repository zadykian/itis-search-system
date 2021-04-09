using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.UserInteraction.Process;

namespace SearchSystem.VectorSearch.Phases
{
	/// <inheritdoc/>
	internal class VectorSearchEnginePhase : ISearchAlgorithmEnginePhase
	{
		// todo: move vectorization to separate phase
		
		private readonly IStatsCollectionSubphase statsCollectionSubphase;
		private readonly ISearchProcess searchProcess;

		public VectorSearchEnginePhase(
			IStatsCollectionSubphase statsCollectionSubphase,
			ISearchProcess searchProcess)
		{
			this.statsCollectionSubphase = statsCollectionSubphase;
			this.searchProcess = searchProcess;
		}

		/// <inheritdoc />
		async Task<Unit> ISearchEnginePhase<ITermsIndex, Unit>.ExecuteAsync(ITermsIndex inputData)
		{
			var termEntryStats = await statsCollectionSubphase.ExecuteAsync(inputData);
			return Unit.Instance;
		}
	}
}