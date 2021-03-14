using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Crawl.Crawler;
using SearchSystem.Crawl.Pages;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.EnginePhases;

namespace SearchSystem.Crawl.Phase
{
	/// <inheritdoc cref="ICrawlEnginePhase"/>
	internal class CrawlEnginePhase : DocumentsOutputPhaseBase<Unit>, ICrawlEnginePhase
	{
		private readonly IWebCrawler webCrawler;

		public CrawlEnginePhase(
			IWebCrawler webCrawler,
			IDocumentStorage documentStorage,
			IAppConfiguration appConfiguration,
			ILogger<CrawlEnginePhase> logger) : base(documentStorage, appConfiguration, logger)
			=> this.webCrawler = webCrawler;

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> CreateNewData(Unit _)
			=> await webCrawler
				.CrawlThroughPages()
				.Zip(AsyncEnumerable.Range(start: 0, int.MaxValue))
				.SelectAwait(async tuple => await SaveWebPageAsDocument(tuple.First, tuple.Second))
				.ToArrayAsync();

		/// <summary>
		/// Save web page to document storage and return saved <see cref="IDocument"/>. 
		/// </summary>
		private async Task<Document> SaveWebPageAsDocument(IWebPage webPage, int pageIndex)
		{
			var document = new Document(ComponentName, $"{pageIndex}.txt", webPage.AllVisibleLines());
			await DocumentStorage.SaveOrAppendAsync(document);

			var indexDocument = new Document(string.Empty, "index.txt", new[] {$"{pageIndex}. {webPage.Url}"});
			await DocumentStorage.SaveOrAppendAsync(indexDocument);

			Logger.LogInformation($"Document '{document.Name}' is saved.");
			return document;
		}
	}
}