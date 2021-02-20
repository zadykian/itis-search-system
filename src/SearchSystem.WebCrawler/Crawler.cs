using System;
using System.Collections.Generic;
using System.Linq;
using SearchSystem.WebCrawler.Pages;

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
		/// Save web pages to destination directory. 
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