using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace SearchSystem.WebCrawler
{
	/// <summary>
	/// Web page.
	/// </summary>
	public class WebPage
	{
		private readonly IDocument document;

		protected WebPage(IDocument document) => this.document = document;

		/// <summary>
		/// Page URI.
		/// </summary>
		public Uri Url => new (document.Url);

		/// <summary>
		/// Page raw content. 
		/// </summary>
		public virtual string RawContent() => document.ToHtml();

		/// <summary>
		/// Page child URLs. 
		/// </summary>
		public virtual IReadOnlyCollection<Uri> ChildUrls()
			=> document
				.All
				.OfType<IHtmlAnchorElement>()
				.Select(element => element.Href)
				.Where(uriString => Uri.TryCreate(uriString, UriKind.Absolute, out _) && uriString != document.Url)
				.Select(uriString => new Uri(uriString))
				.ToImmutableArray();

		/// <summary>
		/// All words visible to user. 
		/// </summary>
		public virtual IReadOnlyCollection<string> AllVisibleWords()
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