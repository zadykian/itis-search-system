using System;
using System.Collections.Generic;
using System.Linq;
using SearchSystem.Common;

namespace SearchSystem.WebCrawler
{
	/// <summary>
	/// Web crawler.
	/// </summary>
	internal class Crawler
	{
		private readonly WebPages webPages = new();
		private readonly Parameters parameters = new();

		/// <summary>
		/// Get web pages which satisfy required conditions.
		/// </summary>
		public IAsyncEnumerable<WebPage> CrawlThrough(Uri rootUri)
			=> webPages
				.Download(rootUri)
				.Where(page => page
					.AllVisibleLines()
					.SelectMany(line => line.Split(separator: ' '))
					.Count() >= parameters.WordsPerPage)
				.Distinct()
				.Take((int) parameters.TotalPages);
	}
}