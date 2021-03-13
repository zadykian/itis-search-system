using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Crawl.Crawler;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.EnginePhases;

namespace SearchSystem.Crawl
{
	/// <inheritdoc />
	public interface ICrawlEnginePhase : ISearchEnginePhase<Unit, IReadOnlyCollection<IDocument>>
	{
	}

	/// <inheritdoc cref="ICrawlEnginePhase"/>
	internal class CrawlEnginePhase : EnginePhaseBase<Unit, IReadOnlyCollection<IDocument>>, ICrawlEnginePhase
	{
		private readonly IWebCrawler webCrawler;

		private const string subsectionName = "crawled-pages";
		private readonly IDocumentStorage documentStorage;

		public CrawlEnginePhase(
			IWebCrawler webCrawler,
			IDocumentStorage documentStorage,
			IAppConfiguration appConfiguration,
			ILogger<CrawlEnginePhase> logger) : base(appConfiguration, logger)
		{
			this.documentStorage = documentStorage;
			this.webCrawler = webCrawler;
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> CreateNewData(Unit _)
		{
			// todo: create index file

			var documents = await webCrawler
				.CrawlThroughPages()
				.Zip(AsyncEnumerable.Range(0, int.MaxValue))
				.Select(tuple => new Document(subsectionName, $"{tuple.Second}.txt", tuple.First.AllVisibleLines()))
				.ToArrayAsync();

			foreach (var document in documents)
			{
				await documentStorage.SaveAsync(document);
			}

			return documents;
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> LoadPreviousResults()
			=> await documentStorage
				.LoadFromSubsection(subsectionName)
				.ToArrayAsync();
	}
}