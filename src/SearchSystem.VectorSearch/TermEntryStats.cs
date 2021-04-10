using System.Text.RegularExpressions;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;
using Sprache;

// ReSharper disable BuiltInTypeReferenceStyle
using Term = System.String;

namespace SearchSystem.VectorSearch
{
	/// <summary>
	/// Struct which contains statistics for term <see cref="TermEntryStats.Term"/>
	/// in context of document <see cref="TermEntryStats.DocumentLink"/>.
	/// </summary>
	internal readonly struct TermEntryStats
	{
		public TermEntryStats(
			Term term,
			IDocumentLink documentLink,
			double termFrequency,
			double inverseDocFrequency)
		{
			Term = term;
			DocumentLink = documentLink;
			TermFrequency = termFrequency;
			InverseDocFrequency = inverseDocFrequency;
		}

		/// <summary>
		/// Term - normalized word entry.
		/// </summary>
		public Term Term { get; }

		/// <summary>
		/// Link to document.
		/// </summary>
		public IDocumentLink DocumentLink { get; }

		/// <summary>
		/// <para>
		/// TF - term frequency.
		/// </para>
		/// <para>
		/// This value represents value ratio between count of entries <see cref="Term"/>
		/// and total words count in document <see cref="DocumentLink"/>.
		/// </para>
		/// </summary>
		private double TermFrequency { get; }

		/// <summary>
		/// <para>
		/// IDF - inverse document frequency.
		/// </para>
		/// <para>
		/// This value represents logarithm of ratio between total documents count and count
		/// of documents which contains term <see cref="Term"/>.
		/// </para>
		/// </summary>
		public double InverseDocFrequency { get; }

		/// <summary>
		/// <para>
		/// TF-IDF - Term frequency — Inverse document frequency.
		/// </para>
		/// <para>
		/// This value represents product of <see cref="TermFrequency"/> and <see cref="InverseDocFrequency"/>.
		/// </para>
		/// </summary>
		public double TfIdf => TermFrequency * InverseDocFrequency;

		/// <inheritdoc />
		public override string ToString()
			=> $"{Term,32} {DocumentLink.Name,8} {TermFrequency,12:F8} {InverseDocFrequency,12:F8} {TfIdf,12:F8}";

		/// <summary>
		/// Parse <paramref name="input"/> to <see cref="TermEntryStats"/> instance. 
		/// </summary>
		public static TermEntryStats ParseLine(string input)
			=> input
				.Trim()
				.To(str => Regex.Replace(str, @"\s+", " "))
				.To(TermStatsGrammar.SingleEntry.Parse);

		/// <summary>
		/// Definition of serialized stats grammar.
		/// </summary>
		private static class TermStatsGrammar
		{
			/// <remarks>
			/// Stats is always calculated based on normalized documents.
			/// </remarks>
			private const string subsectionName = "Normalization";

			/// <summary>
			/// Single <see cref="TermEntryStats"/> parser.
			/// </summary>
			public static Parser<TermEntryStats> SingleEntry =>
				from term                in Parse.CharExcept(' ').Many().Text()
				from documentLink        in Parse
					.Regex(@"\d+\.[a-z]{3}")
					.Token()
					.Select(fileName => new DocumentLink(subsectionName, fileName))
				from termFrequency       in DoubleParser
				from inverseDocFrequency in DoubleParser
				select new TermEntryStats(term, documentLink, termFrequency, inverseDocFrequency);

			/// <summary>
			/// Double value parser.
			/// </summary>
			private static Parser<double> DoubleParser => Parse.DecimalInvariant.Select(double.Parse).Token();
		}
	}
}