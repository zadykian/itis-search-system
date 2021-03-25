using System;
using System.Collections.Generic;

// ReSharper disable BuiltInTypeReferenceStyle
using PageId = System.UInt32;

namespace SearchSystem.Infrastructure.WebPages
{
	/// <summary>
	/// Web pages index.
	/// </summary>
	public interface IWebPagesIndex
	{
		/// <summary>
		/// Pages being saved during crawl phase.
		/// </summary>
		IReadOnlyDictionary<PageId, Uri> SavedPages { get; }
	}
}