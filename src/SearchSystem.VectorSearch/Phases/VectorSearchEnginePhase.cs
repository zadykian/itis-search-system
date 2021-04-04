using System;
using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.VectorSearch.Phases
{
	/// <inheritdoc/>
	internal class VectorSearchEnginePhase : ISearchAlgorithmEnginePhase
	{
		private readonly IStatsCollectionSubphase statsCollectionSubphase;

		public VectorSearchEnginePhase(IStatsCollectionSubphase statsCollectionSubphase)
			=> this.statsCollectionSubphase = statsCollectionSubphase;

		/// <inheritdoc />
		Task<Unit> ISearchEnginePhase<ITermsIndex, Unit>.ExecuteAsync(ITermsIndex inputData) => throw new NotImplementedException();
	}
}