using System.Collections.Generic;
using SearchSystem.Common;

namespace SearchSystem.Crawl
{
	/// <summary>
	/// Web crawler.
	/// </summary>
	public interface IWebCrawler
	{
		/// <summary>
		/// Get web pages which satisfy required conditions.
		/// </summary>
		IAsyncEnumerable<IWebPage> CrawlThroughPages();
	}
}