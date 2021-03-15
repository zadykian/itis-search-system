using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.EnginePhases;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Indexing.Phase
{
	/// <inheritdoc cref="IIndexingEnginePhase"/>
	internal class IndexingEnginePhase : EnginePhaseBase<Docs, IDocumentsIndex>, IIndexingEnginePhase
	{
		public IndexingEnginePhase(
			IAppConfiguration appConfiguration,
			ILogger<IndexingEnginePhase> logger) : base(appConfiguration, logger)
		{
		}

		/// <inheritdoc />
		protected override Task<IDocumentsIndex> CreateNewData(Docs inputData) => throw new System.NotImplementedException();

		/// <inheritdoc />
		protected override Task<IDocumentsIndex> LoadPreviousResults() => throw new System.NotImplementedException();
	}
}