using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc/>
	/// <remarks>
	/// This phase provides functionality of boolean search
	/// based on prebuilt index <see cref="ITermsIndex"/>.
	/// </remarks>
	public interface IBooleanSearchEnginePhase : ISearchEnginePhase<ITermsIndex, Unit>
	{
	}
}