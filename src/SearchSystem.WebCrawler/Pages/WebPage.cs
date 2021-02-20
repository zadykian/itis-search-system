using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace SearchSystem.WebCrawler.Pages
{
	/// <summary>
	/// Web page.
	/// </summary>
	internal class WebPage : IEquatable<WebPage>
	{
		private readonly IDocument document;

		protected WebPage(IDocument document) => this.document = document;

		/// <summary>
		/// Page URI.
		/// </summary>
		public Uri Url => new (document.Url);

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
		/// All text lines visible to user. 
		/// </summary>
		public virtual IReadOnlyCollection<string> AllVisibleLines()
			=> document
				.All
				.Where(element => element
					is IHtmlSpanElement
					or IHtmlTitleElement
					or IHtmlAnchorElement
					|| element is IHtmlListItemElement && element.ChildElementCount == 1
					)
				.Select(element => Regex.Replace(element.Text(), @"\s+", " "))
				.Where(elementText => !string.IsNullOrWhiteSpace(elementText))
				.Distinct()
				.ToImmutableArray();

		/// <inheritdoc />
		public bool Equals(WebPage? other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return document.Url.Equals(other.document.Url);
		}

		/// <inheritdoc />
		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((WebPage) obj);
		}

		/// <inheritdoc />
		public override int GetHashCode() => document.Url.GetHashCode();

		/// <inheritdoc />
		public override string ToString() => document.Url;
	}
}