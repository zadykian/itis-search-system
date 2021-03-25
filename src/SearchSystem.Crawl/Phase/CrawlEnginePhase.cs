using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Crawl.Crawler;
using SearchSystem.Crawl.Pages;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Conventions;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.Crawl.Phase
{
	/// <inheritdoc cref="ICrawlEnginePhase"/>
	internal class CrawlEnginePhase : DocumentsOutputPhaseBase<Unit>, ICrawlEnginePhase
	{
		private readonly IWebCrawler webCrawler;
		private readonly IStorageConventions storageConventions;

		public CrawlEnginePhase(
			IWebCrawler webCrawler,
			IDocumentStorage documentStorage,
			IStorageConventions storageConventions,
			IAppEnvironment<CrawlEnginePhase> appEnvironment) : base(documentStorage, appEnvironment)
		{
			this.webCrawler = webCrawler;
			this.storageConventions = storageConventions;
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> ExecuteAnewAsync(Unit _)
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

			var indexDocument = storageConventions.WebPagesIndex.ToDocument($"{pageIndex}. {webPage.Url}");
			await DocumentStorage.SaveOrAppendAsync(indexDocument);

			Environment.Logger.LogInformation($"Document '{document.Name}' ({webPage.Url}) is saved.");
			return document;
		}
	}
}