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
		public Uri Uri => new (document.Url);

		/// <summary>
		/// Page child URLs. 
		/// </summary>
		public virtual IReadOnlyCollection<Uri> ChildUrls()
			=> document
				.All
				.OfType<IHtmlAnchorElement>()
				.Where(anchorElement => anchorElement.ClassName is null)
				.Select(element => element.Href)
				.Where(uriString =>
					Uri.TryCreate(uriString, UriKind.Absolute, out var uri)
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
				.Where(element => element is not IHtmlScriptElement && element.ChildElementCount == 0)
				.Select(element => Regex.Replace(element.Text(), @"\s+", " "))
				.Where(elementText => !string.IsNullOrWhiteSpace(elementText))
				.ToImmutableArray();

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