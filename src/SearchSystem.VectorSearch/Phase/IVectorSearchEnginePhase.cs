using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.VectorSearch.Phase
{
	/// <inheritdoc/>
	/// <remarks>
	/// This phase provides functionality of vector search
	/// based on prebuilt index <see cref="ITermsIndexEnumerable"/>.
	/// </remarks>
	public interface IVectorSearchEnginePhase : ISearchEnginePhase<ITermsIndexEnumerable, Unit>
	{
	}
}