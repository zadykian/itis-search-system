using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Crawl.Crawler;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Storage;
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
				.Zip(AsyncEnumerable.Range(start: 0, int.MaxValue))
				.SelectAwait(async tuple =>
				{
					var (webPage, pageIndex) = tuple;
					var document = new Document(subsectionName, $"{pageIndex}.txt", webPage.AllVisibleLines());
					await documentStorage.SaveOrAppendAsync(document);
					return document;
				})
				.ToArrayAsync();

			return documents;
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> LoadPreviousResults()
			=> await documentStorage
				.LoadFromSubsection(subsectionName)
				.ToArrayAsync();
	}
}