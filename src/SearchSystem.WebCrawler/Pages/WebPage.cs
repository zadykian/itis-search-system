using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
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
		public Uri Uri => new (document.Url);

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
					&& !Path.HasExtension(uriString)
					&& uri != Uri
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
				.Where(element => element is not (
					IHtmlScriptElement
					or IHtmlHtmlElement
					or IHtmlHeadElement
					or IHtmlBodyElement
					or IHtmlStyleElement
					or IHtmlDivElement)
				                  && element.GetType().Name != "HtmlSemanticElement"
				                  && HasOneOrZeroChild(element))
				.Select(element =>
				{
					var current = element;
					while (current.ChildElementCount == 1)
					{
						current = current.Children.First();
					}

					return current;
				})
				.Distinct()
				.Where(element => !string.IsNullOrWhiteSpace(element.TextContent))
				.Select(element => Regex.Replace(element.TextContent, @"\n+", "\n"))
				.Select(text => Regex.Replace(text, @"\t+", "\t"))
				.Where(elementText => !string.IsNullOrWhiteSpace(elementText))
				.ToImmutableArray();

		private static bool HasOneOrZeroChild(IElement element)
		{
			while (element.ChildElementCount == 1)
			{
				element = element.Children.First();
			}

			return element.ChildElementCount == 0;
		}

		/// <inheritdoc />
		public bool Equals(WebPage? other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Uri.Equals(other.Uri);
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
		public override int GetHashCode() => Uri.GetHashCode();

		/// <inheritdoc />
		public override string ToString() => Uri.ToString();
	}
}