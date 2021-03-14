using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using SearchSystem.Crawl.Pages;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.Crawl.Crawler
{
	/// <inheritdoc />
	internal class WebCrawler : IWebCrawler
	{
		private readonly IAppConfiguration appConfiguration;

		public WebCrawler(IAppConfiguration appConfiguration) => this.appConfiguration = appConfiguration;

		/// <inheritdoc />
		IAsyncEnumerable<IWebPage> IWebCrawler.CrawlThroughPages()
			=> appConfiguration
				.RootPageUri()
				.To(WebPages.Download)
				.Where(page => page
					.AllVisibleLines()
					.SelectMany(line => line.Words())
					.Count() >= appConfiguration.WordsPerPage())
				.Distinct()
				.Take((int) appConfiguration.TotalPages());

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