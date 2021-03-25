using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Crawl.Crawler;
using SearchSystem.Crawl.Pages;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Infrastructure.WebPages;

// ReSharper disable BuiltInTypeReferenceStyle
using PageId = System.UInt32;

namespace SearchSystem.Crawl.Phase
{
	/// <inheritdoc cref="ICrawlEnginePhase"/>
	internal class CrawlEnginePhase : DocumentsOutputPhaseBase<Unit>, ICrawlEnginePhase
	{
		private readonly IWebCrawler webCrawler;

		public CrawlEnginePhase(
			IWebCrawler webCrawler,
			IAppEnvironment<CrawlEnginePhase> appEnvironment) : base(appEnvironment)
			=> this.webCrawler = webCrawler;

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> ExecuteAnewAsync(Unit _)
			=> await webCrawler
				.CrawlThroughPages()
				.Zip(AsyncEnumerable
					.Range(start: 0, int.MaxValue)
					.Select(intValue => (PageId) intValue))
				.SelectAwait(async tuple => await SaveWebPageAsDocument(tuple.First, tuple.Second))
				.ToArrayAsync();

		/// <summary>
		/// Save web page to document storage and return saved <see cref="IDocument"/>. 
		/// </summary>
		private async Task<Document> SaveWebPageAsDocument(IWebPage webPage, PageId pageId)
		{
			var document = new Document(ComponentName, $"{pageId}.txt", webPage.AllVisibleLines());
			await AppEnvironment.Storage.SaveOrAppendAsync(document);

			var webPagesIndex = new WebPagesIndex(pageId, webPage.Url);
			var indexDocument = webPagesIndex.AsDocument(AppEnvironment.Storage.Conventions.WebPagesIndex);
			await AppEnvironment.Storage.SaveOrAppendAsync(indexDocument);

			AppEnvironment.Logger.LogInformation($"Document '{document.Name}' ({webPage.Url}) is saved.");
			return document;
		}
	}
}