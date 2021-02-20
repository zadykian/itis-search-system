using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SearchSystem.WebCrawler.Pages
{
	/// <summary>
	/// Representation of multiple web pages.
	/// </summary>
	internal class WebPages
	{
		/// <summary>
		/// Download web page located at <paramref name="pageUri"/> recursively with referenced ones.
		/// </summary>
		public IAsyncEnumerable<WebPage> Download(Uri pageUri) => Download(pageUri, withCurrent: true);

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
			Console.WriteLine($"{pageUri} is downloaded.");
			var htmlDocument = await new HtmlWeb().LoadFromWebAsync(pageUri.ToString());
			return new CachedWebPage(pageUri, htmlDocument);
		}
	}
}