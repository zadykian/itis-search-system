using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;
using Sprache;

// ReSharper disable BuiltInTypeReferenceStyle
using PageId = System.UInt32;

namespace SearchSystem.Infrastructure.WebPages
{
	/// <summary>
	/// Web pages index.
	/// </summary>
	public class WebPagesIndex
	{
		public WebPagesIndex(PageId pageId, Uri pageUri)
			=> SavedPages = new[] {(pageId, pageUri)};

		public WebPagesIndex(IDocument document)
			=> SavedPages = document
				.Lines
				.Select(Grammar.Line.Parse)
				.ToImmutableArray();

		/// <summary>
		/// Pages being saved during crawl phase.
		/// </summary>
		public IReadOnlyCollection<(PageId PageId, Uri PageUri)> SavedPages { get; }

		/// <summary>
		/// Represent itself as <see cref="IDocument"/> instance. 
		/// </summary>
		public IDocument AsDocument(IDocumentLink documentLink)
			=> SavedPages
				.Select(pair => $"{pair.PageId}. {pair.PageUri}")
				.ToArray()
				.To(documentLink.ToDocument);

		/// <summary>
		/// Definition of serialized index grammar.
		/// </summary>
		private static class Grammar
		{
			/// <summary>
			/// Single line parser.
			/// </summary>
			public static Parser<(PageId PageId, Uri PageUri)> Line =>
				from pageId    in Parse.Number.Select(PageId.Parse)
				from separator in Parse.String(". ")
				from pageUri   in Parse.AnyChar.Many().Text().Select(str => new Uri(str))
				select (pageId, pageUri);
		}
	}
}