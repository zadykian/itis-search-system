using System;
using System.Collections.Generic;

namespace SearchSystem.Crawl.Pages
{
	/// <summary>
	/// Web page.
	/// </summary>
	public interface IWebPage
	{
		/// <summary>
		/// Page URI.
		/// </summary>
		Uri Url { get; }

		/// <summary>
		/// All text lines visible to user. 
		/// </summary>
		IReadOnlyCollection<string> AllVisibleLines();

		/// <summary>
		/// Page child URLs. 
		/// </summary>
		IReadOnlyCollection<Uri> ChildUrls();
	}
}