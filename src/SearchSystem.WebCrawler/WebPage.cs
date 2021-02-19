using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SearchSystem.WebCrawler
{
	public readonly struct WebPage
	{
		public WebPage(
			Uri uri,
			string rawContent,
			IEnumerable<Uri> childrenUrls)
		{
			Uri = uri;
			RawContent = rawContent;
			ChildrenUrls = childrenUrls.ToImmutableArray();
		}

		public Uri Uri { get; }

		public string RawContent { get; }

		public IReadOnlyCollection<Uri> ChildrenUrls { get; }
	}
}