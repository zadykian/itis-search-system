using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;

namespace SearchSystem.WebCrawler
{
	internal class WebPages
	{
		/// <summary>
		/// Download web page located at <paramref name="pageUri"/> recursively with referenced ones.
		/// </summary>
		public async IAsyncEnumerable<WebPage> Download(Uri pageUri, bool withCurrent = true)
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