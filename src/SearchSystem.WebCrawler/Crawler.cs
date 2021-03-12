using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using SearchSystem.Common.Configuration;

namespace SearchSystem.WebCrawler
{
	/// <summary>
	/// Web crawler.
	/// </summary>
	internal class Crawler
	{
		private readonly IAppConfiguration appConfiguration;

		public Crawler(IAppConfiguration appConfiguration) => this.appConfiguration = appConfiguration;

		/// <summary>
		/// Get web pages which satisfy required conditions.
		/// </summary>
		public IAsyncEnumerable<WebPage> CrawlThrough(Uri rootUri)
			=> WebPages
				.Download(rootUri)
				.Where(page => page
					.AllVisibleLines()
					.SelectMany(line => line.Split(separator: ' '))
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
			public static IAsyncEnumerable<WebPage> Download(Uri pageUri) => Download(pageUri, withCurrent: true);

			/// <inheritdoc cref="Download(Uri)"/>
			private static async IAsyncEnumerable<WebPage> Download(Uri pageUri, bool withCurrent)
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
			private static async Task<WebPage> DownloadSingle(Uri pageUri)
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