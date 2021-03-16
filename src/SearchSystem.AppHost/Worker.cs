using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
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

		public Worker(
			ICrawlEnginePhase crawlEnginePhase,
			INormalizationEnginePhase normalizationEnginePhase,
			IIndexingEnginePhase indexingEnginePhase)
		{
			this.crawlEnginePhase = crawlEnginePhase;
			this.normalizationEnginePhase = normalizationEnginePhase;
			this.indexingEnginePhase = indexingEnginePhase;
		}

		/// <inheritdoc />
		protected override Task ExecuteAsync(CancellationToken stoppingToken)
			=> Composable
				.Add<Unit, Task<Docs>>(crawlEnginePhase.ExecuteAsync)
				.Add(normalizationEnginePhase.ExecuteAsync)
				.Add(indexingEnginePhase.ExecuteAsync)
				.Invoke(Unit.Instance);
	}
}