using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SearchSystem.BooleanSearch.Phase;
using SearchSystem.Crawl.Phase;
using SearchSystem.Indexing.Phase;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Phase;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.AppHost
{
	/// <inheritdoc />
	internal class Worker : BackgroundService
	{
		private readonly ICrawlEnginePhase crawlEnginePhase;
		private readonly INormalizationEnginePhase normalizationEnginePhase;
		private readonly IIndexingEnginePhase indexingEnginePhase;
		private readonly IBooleanSearchEnginePhase booleanSearchEnginePhase;

		public Worker(
			ICrawlEnginePhase crawlEnginePhase,
			INormalizationEnginePhase normalizationEnginePhase,
			IIndexingEnginePhase indexingEnginePhase,
			IBooleanSearchEnginePhase booleanSearchEnginePhase)
		{
			this.crawlEnginePhase = crawlEnginePhase;
			this.normalizationEnginePhase = normalizationEnginePhase;
			this.indexingEnginePhase = indexingEnginePhase;
			this.booleanSearchEnginePhase = booleanSearchEnginePhase;
		}

		/// <inheritdoc />
		protected override Task ExecuteAsync(CancellationToken stoppingToken)
			=> Composable
				.Add<Unit, Task<Docs>>(crawlEnginePhase.ExecuteAsync)
				.Add(normalizationEnginePhase.ExecuteAsync)
				.Add(indexingEnginePhase.ExecuteAsync)
				.Add(booleanSearchEnginePhase.ExecuteAsync)
				.Invoke(Unit.Instance);
	}
}