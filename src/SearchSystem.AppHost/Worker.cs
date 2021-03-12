using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SearchSystem.Crawl;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.EnginePhases;
using SearchSystem.Lemmatization;

namespace SearchSystem.AppHost
{
	/// <inheritdoc />
	internal class Worker : BackgroundService
	{
		private readonly ICrawlEnginePhase crawlEnginePhase;
		private readonly ILemmatizationEnginePhase lemmatizationEnginePhase;

		public Worker(
			ICrawlEnginePhase crawlEnginePhase,
			ILemmatizationEnginePhase lemmatizationEnginePhase)
		{
			this.crawlEnginePhase = crawlEnginePhase;
			this.lemmatizationEnginePhase = lemmatizationEnginePhase;
		}

		/// <inheritdoc />
		protected override Task ExecuteAsync(CancellationToken stoppingToken)
			=> Composable
				.Add<Unit, Task<IReadOnlyCollection<IDocument>>>(crawlEnginePhase.ExecuteAsync)
				.Add(lemmatizationEnginePhase.ExecuteAsync)
				.Invoke(Unit.Instance);
	}
}