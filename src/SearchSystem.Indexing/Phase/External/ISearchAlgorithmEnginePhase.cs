using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.Indexing.Phase.External
{
	/// <inheritdoc/>
	/// <remarks>
	/// This phase provides functionality of search
	/// based on prebuilt index <see cref="ITermsIndex"/>.
	/// </remarks>
	public interface ISearchAlgorithmEnginePhase : ISearchEnginePhase<ITermsIndex, Unit>
	{
	}
}