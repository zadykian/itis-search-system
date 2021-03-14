using SearchSystem.Infrastructure.EnginePhases;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Normalization.Phase
{
	/// <inheritdoc />
	public interface INormalizationEnginePhase : ISearchEnginePhase<Docs, Docs>
	{
	}
}