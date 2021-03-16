using System.Collections.Generic;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.Crawl.Phase
{
	/// <inheritdoc />
	public interface ICrawlEnginePhase : ISearchEnginePhase<Unit, IReadOnlyCollection<IDocument>>
	{
	}
}