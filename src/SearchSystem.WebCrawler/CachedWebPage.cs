using System;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace SearchSystem.WebCrawler
{
	/// <inheritdoc />
	public class CachedWebPage : WebPage
	{
		private string? rawContent;
		private IReadOnlyCollection<Uri>? childUrls;
		private IReadOnlyCollection<string>? allVisibleWords;

		public CachedWebPage(IDocument document) : base(document)
		{
		}

		/// <inheritdoc />
		public override string RawContent() => rawContent ??= base.RawContent();

		/// <inheritdoc />
		public override IReadOnlyCollection<Uri> ChildUrls() => childUrls ??= base.ChildUrls();

		/// <inheritdoc />
		public override IReadOnlyCollection<string> AllVisibleWords() => allVisibleWords ??= base.AllVisibleWords();
	}
}