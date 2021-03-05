using System;
using System.Collections.Generic;

namespace SearchSystem.Common
{
	/// <summary>
	/// Web page.
	/// </summary>
	public interface IWebPage
	{
		/// <summary>
		/// Page URI.
		/// </summary>
		Uri Uri { get; }

		/// <summary>
		/// All text lines visible to user. 
		/// </summary>
		IReadOnlyCollection<string> AllVisibleLines();
	}
}