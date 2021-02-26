using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SearchSystem.WebCrawler.Pages
{
	/// <summary>
	/// Web page.
	/// </summary>
	internal class WebPage : IEquatable<WebPage>
	{
		private readonly HtmlDocument htmlDocument;

		protected WebPage(Uri pageUrl, HtmlDocument htmlDocument)
		{
			Url = pageUrl;
			this.htmlDocument = htmlDocument;
		}

		/// <summary>
		/// Page URI.
		/// </summary>
		public Uri Url { get; }

		/// <summary>
		/// Page child URLs. 
		/// </summary>
		public virtual IReadOnlyCollection<Uri> ChildUrls()
			=> htmlDocument
				.DocumentNode
				.SelectNodes("//a[@href]")
				.Select(htmlNode => htmlNode.GetAttributeValue("href", string.Empty))
				.Where(uriString => Uri.TryCreate(uriString, UriKind.Absolute, out _) && uriString != Url.ToString() && uriString.StartsWith("http"))
				.Distinct()
				.Select(uriString => new Uri(uriString))
				.ToImmutableArray();

		/// <summary>
		/// All text lines visible to user. 
		/// </summary>
		public virtual IReadOnlyCollection<string> AllVisibleLines()
			=> htmlDocument
				.DocumentNode
				.SelectNodes("//text()")
				.Select(htmlNode => Regex.Replace(htmlNode.InnerText, @"\s+", " "))
				.Where(innerText => !string.IsNullOrWhiteSpace(innerText))
				.ToImmutableArray();

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
			if (obj.GetType() != this.GetType()) return false;
			return Equals((WebPage) obj);
		}

		/// <inheritdoc />
		public override int GetHashCode() => Url.GetHashCode();

		/// <inheritdoc />
		public override string ToString() => Url.ToString();
	}
}