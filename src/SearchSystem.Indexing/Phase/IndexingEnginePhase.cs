using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.EnginePhases;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Indexing.Phase
{
	/// <inheritdoc cref="IIndexingEnginePhase"/>
	internal class IndexingEnginePhase : EnginePhaseBase<Docs, IDocumentsIndex>, IIndexingEnginePhase
	{
		public IndexingEnginePhase(IAppEnvironment<IndexingEnginePhase> appEnvironment) : base(appEnvironment)
		{
		}

		/// <inheritdoc />
		protected override Task<IDocumentsIndex> CreateNewData(Docs inputData)
			=> throw new System.NotImplementedException();

		/// <inheritdoc />
		protected override Task<IDocumentsIndex> LoadPreviousResults()
			=> throw new System.NotImplementedException();
	}
}