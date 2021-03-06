using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace SearchSystem.Crawl.Pages
{
	/// <summary>
	/// Web page.
	/// </summary>
	internal class WebPage : IWebPage, IEquatable<WebPage>
	{
		private readonly IDocument document;

		protected WebPage(IDocument document) => this.document = document;

		/// <summary>
		/// Page URI.
		/// </summary>
		public Uri Url => new(document.Url);

		/// <summary>
		/// Page child URLs. 
		/// </summary>
		public virtual IReadOnlyCollection<Uri> ChildUrls()
			=> document
				.All
				.OfType<IHtmlAnchorElement>()
				.Select(element => element.Href)
				.Where(uriString =>
					Uri.TryCreate(uriString, UriKind.Absolute, out var uri)
					&& (!Path.HasExtension(uriString) || Path.GetExtension(uriString).ToLower() == "html")
					&& uri != Url
					&& (uri.Scheme == "http" || uri.Scheme == "https"))
				.Distinct()
				.Select(uriString => new Uri(uriString))
				.ToImmutableArray();

		/// <summary>
		/// All text lines visible to user. 
		/// </summary>
		public virtual IReadOnlyCollection<string> AllVisibleLines()
			=> document
				.All
				.Where(ElementPredicate)
				.Select(element => element.Flatten())
				.Distinct()
				.Where(element => !string.IsNullOrWhiteSpace(element.TextContent))
				.Select(element => element.TextContent)
				.Select(text => Regex.Replace(text, @"\n+", "\n"))
				.Select(text => Regex.Replace(text, "( |\t)+", " "))
				.Select(text => text.Trim())
				.ToImmutableArray();

		/// <summary>
		/// Predicate to filter DOM elements. 
		/// </summary>
		private static bool ElementPredicate(IElement element)
			=> element is not (
				   IHtmlScriptElement
				   or IHtmlHtmlElement
				   or IHtmlHeadElement
				   or IHtmlBodyElement
				   or IHtmlStyleElement
				   or IHtmlDivElement)
			   && element.GetType().Name != "HtmlSemanticElement"
			   && element.HasZeroOrOneChild();

		/// <inheritdoc />
		public bool Equals(WebPage? other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Url.Equals(other.Url);
		}

		/// <inheritdoc />
		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((WebPage) obj);
		}

		/// <inheritdoc />
		public override int GetHashCode() => Url.GetHashCode();

		/// <inheritdoc />
		public override string ToString() => Url.ToString();
	}
}