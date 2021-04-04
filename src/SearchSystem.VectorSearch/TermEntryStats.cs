using System;
using SearchSystem.Infrastructure.Documents;

// ReSharper disable BuiltInTypeReferenceStyle
using Term = System.String;

namespace SearchSystem.VectorSearch
{
	/// <summary>
	/// Struct which contains statistics for term <see cref="TermEntryStats.Term"/>
	/// in context of document <see cref="TermEntryStats.DocumentLink"/>.
	/// </summary>
	public readonly struct TermEntryStats
	{
		public TermEntryStats(
			String term,
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
		private double InverseDocFrequency { get; }

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
	}
}