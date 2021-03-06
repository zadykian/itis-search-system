using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.Extensions.Logging;
using SearchSystem.Crawl.Pages;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.Words;

namespace SearchSystem.Crawl.Crawler
{
	/// <inheritdoc />
	internal class WebCrawler : IWebCrawler
	{
		private readonly IAppEnvironment<WebCrawler> appEnvironment;
		private readonly IWordExtractor wordExtractor;

		public WebCrawler(
			IAppEnvironment<WebCrawler> appEnvironment,
			IWordExtractor wordExtractor)
		{
			this.appEnvironment = appEnvironment;
			this.wordExtractor = wordExtractor;
		}

		/// <inheritdoc />
		IAsyncEnumerable<IWebPage> IWebCrawler.CrawlThroughPages()
			=> appEnvironment
				.Configuration
				.RootPageUri()
				.To(WebPages.Download)
				.Where(WebPagePredicate)
				.Distinct()
				.Take((int) appEnvironment.Configuration.TotalPages());

		/// <summary>
		/// Predicate for filtering pages by <see cref="IAppConfiguration.WordsPerPage"/> config value. 
		/// </summary>
		private bool WebPagePredicate(IWebPage page)
		{
			var wordsCount = page
				.AllVisibleLines()
				.SelectMany(line => wordExtractor.Parse(line))
				.Count();

			if (wordsCount >= appEnvironment.Configuration.WordsPerPage())
			{
				return true;
			}

			appEnvironment.Logger.LogTrace($"Page '{page.Url}' is skipped (contains {wordsCount} words).");
			return false;
		}

		/// <summary>
		/// Representation of multiple web pages.
		/// </summary>
		private static class WebPages
		{
			/// <summary>
			/// Download web page located at <paramref name="pageUri"/> recursively with referenced ones.
			/// </summary>
			public static IAsyncEnumerable<IWebPage> Download(Uri pageUri) => Download(pageUri, withCurrent: true);

			/// <inheritdoc cref="Download(Uri)"/>
			private static async IAsyncEnumerable<IWebPage> Download(Uri pageUri, bool withCurrent)
			{
				var currentPage = await DownloadSingle(pageUri);

				if (withCurrent)
				{
					yield return currentPage;
				}

				foreach (var childUrl in currentPage.ChildUrls())
				{
					yield return await DownloadSingle(childUrl);
				}

				foreach (var childUrl in currentPage.ChildUrls())
				{
					await foreach (var childPage in Download(childUrl, withCurrent: false))
					{
						yield return childPage;
					}
				}
			}

			/// <summary>
			/// Download web page located at <paramref name="pageUri"/>.
			/// </summary>
			private static async Task<IWebPage> DownloadSingle(Uri pageUri)
			{
				var configuration = Configuration.Default.WithDefaultLoader();

				var document = await BrowsingContext
					.New(configuration)
					.OpenAsync(pageUri.ToString());

				return new CachedWebPage(document);
			}
		}
	}
}