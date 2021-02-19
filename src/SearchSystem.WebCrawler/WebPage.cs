using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace SearchSystem.WebCrawler
{
	public class WebPage
	{
		private readonly IDocument document;

		public WebPage(IDocument document) => this.document = document;

		public Uri Uri => new (document.Url);

		public string RawContent() => document.ToHtml();

		public IReadOnlyCollection<Uri> ChildrenUrls()
			=> document
				.All
				.OfType<IHtmlAnchorElement>()
				.Select(element => element.Href)
				.Where(uriString => Uri.TryCreate(uriString, UriKind.Absolute, out _) && uriString != document.Url)
				.Select(uriString => new Uri(uriString))
				.ToImmutableArray();

		public IEnumerable<string> AllVisibleWords()
			=> document
				.All
				.Where(element => element
					is IHtmlSpanElement
					or IHtmlDivElement
					or IHtmlTitleElement
					or IHtmlAnchorElement)
				.Select(element => element.Text())
				.Where(elementText => !string.IsNullOrWhiteSpace(elementText))
				.SelectMany(elementText => elementText.Split(separator: ' '))
				.ToImmutableArray();
	}
}