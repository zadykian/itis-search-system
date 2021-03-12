using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SearchSystem.Crawl;
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
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await Task.CompletedTask;
			
			
		}
	}
}