using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SearchSystem.Crawl;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.EnginePhases;
using SearchSystem.Normalization;

namespace SearchSystem.AppHost
{
	/// <inheritdoc />
	internal class Worker : BackgroundService
	{
		private readonly ICrawlEnginePhase crawlEnginePhase;
		private readonly INormalizationEnginePhase normalizationEnginePhase;

		public Worker(
			ICrawlEnginePhase crawlEnginePhase,
			INormalizationEnginePhase normalizationEnginePhase)
		{
			this.crawlEnginePhase = crawlEnginePhase;
			this.normalizationEnginePhase = normalizationEnginePhase;
		}

		/// <inheritdoc />
		protected override Task ExecuteAsync(CancellationToken stoppingToken)
			=> Composable
				.Add<Unit, Task<IReadOnlyCollection<IDocument>>>(crawlEnginePhase.ExecuteAsync)
				.Add(normalizationEnginePhase.ExecuteAsync)
				.Invoke(Unit.Instance);
	}
}