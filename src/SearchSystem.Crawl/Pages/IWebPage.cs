using System;
using System.Collections.Generic;

namespace SearchSystem.Crawl.Pages
{
	public interface IFile
	{
		string Name { get; }

		IReadOnlyCollection<string> AllLines();
	}


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