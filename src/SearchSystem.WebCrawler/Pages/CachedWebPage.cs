using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace SearchSystem.WebCrawler.Pages
{
	/// <inheritdoc />
	internal class CachedWebPage : WebPage
	{
		private IReadOnlyCollection<Uri>? childUrls;
		private IReadOnlyCollection<string>? allVisibleWords;

		public CachedWebPage(Uri pageUrl, HtmlDocument htmlDocument) : base(pageUrl, htmlDocument)
		{
		}

		/// <inheritdoc />
		public override IReadOnlyCollection<Uri> ChildUrls() => childUrls ??= base.ChildUrls();

		/// <inheritdoc />
		public override IReadOnlyCollection<string> AllVisibleLines() => allVisibleWords ??= base.AllVisibleLines();
	}
}