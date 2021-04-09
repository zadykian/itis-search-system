using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SearchSystem.Crawl.Phase;
using SearchSystem.Indexing.Phase;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Phase;

namespace SearchSystem.AppHost
{
	/// <inheritdoc />
	internal class Worker : BackgroundService
	{
		private readonly ICrawlEnginePhase crawlEnginePhase;
		private readonly INormalizationEnginePhase normalizationEnginePhase;
		private readonly IIndexingEnginePhase indexingEnginePhase;
		private readonly ISearchAlgorithmEnginePhase algorithmEnginePhase;

		public Worker(
			ICrawlEnginePhase crawlEnginePhase,
			INormalizationEnginePhase normalizationEnginePhase,
			IIndexingEnginePhase indexingEnginePhase,
			ISearchAlgorithmEnginePhase algorithmEnginePhase)
		{
			this.crawlEnginePhase = crawlEnginePhase;
			this.normalizationEnginePhase = normalizationEnginePhase;
			this.indexingEnginePhase = indexingEnginePhase;
			this.algorithmEnginePhase = algorithmEnginePhase;
		}

		/// <inheritdoc />
		protected override Task ExecuteAsync(CancellationToken _)
			=> Composable
				.Add(crawlEnginePhase.ExecuteAsync)
				.Add(normalizationEnginePhase.ExecuteAsync)
				.Add(indexingEnginePhase.ExecuteAsync)
				.Add(algorithmEnginePhase.ExecuteAsync)
				.Invoke(Unit.Instance);
	}
}