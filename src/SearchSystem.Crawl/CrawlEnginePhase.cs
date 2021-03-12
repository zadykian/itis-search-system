using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Crawl.Crawler;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.EnginePhases;

namespace SearchSystem.Crawl
{
	internal class CrawlEnginePhase : EnginePhaseBase<Unit, IReadOnlyCollection<IDocument>>
	{
		private readonly IWebCrawler webCrawler;

		private const string subsectionName = "crawled-pages";
		private readonly IDocumentStorage documentStorage;

		public CrawlEnginePhase(
			IWebCrawler webCrawler,
			IDocumentStorage documentStorage,
			IAppConfiguration appConfiguration) : base(appConfiguration)
		{
			this.documentStorage = documentStorage;
			this.webCrawler = webCrawler;
		}

		/// <inheritdoc />
		protected override string ComponentName => "Crawl";

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> CreateNewData(Unit _)
		{
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