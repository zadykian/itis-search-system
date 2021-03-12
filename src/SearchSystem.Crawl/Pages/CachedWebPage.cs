using System;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace SearchSystem.Crawl.Pages
{
	/// <inheritdoc />
	internal class CachedWebPage : WebPage
	{
		private IReadOnlyCollection<Uri>? childUrls;
		private IReadOnlyCollection<string>? allVisibleWords;

		public CachedWebPage(IDocument document) : base(document)
		{
		}

		/// <inheritdoc />
		public override IReadOnlyCollection<Uri> ChildUrls() => childUrls ??= base.ChildUrls();

		/// <inheritdoc />
		public override IReadOnlyCollection<string> AllVisibleLines() => allVisibleWords ??= base.AllVisibleLines();
	}
}