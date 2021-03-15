using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.EnginePhases;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Indexing.Phase
{
	/// <inheritdoc />
	/// <remarks>
	/// This phase performs normalized documents indexing.
	/// </remarks>
	public interface IIndexingEnginePhase : ISearchEnginePhase<Docs, IDocumentsIndex>
	{
	}
}