using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.EnginePhases;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc />
	/// <remarks>
	/// This phase provides functionality of boolean search
	/// based on prebuilt index <see cref="IDocumentsIndex"/>.
	/// </remarks>
	public interface IBooleanSearchEnginePhase : ISearchEnginePhase<IDocumentsIndex, Unit>
	{
	}
}