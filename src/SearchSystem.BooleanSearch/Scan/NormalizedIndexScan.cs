using System.Collections.Generic;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Normalization.Normalizer;

namespace SearchSystem.BooleanSearch.Scan
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementations performs normalization of terms in <see cref="INode"/> expression
	/// before action index scanning.
	/// </remarks>
	internal class NormalizedIndexScan : IIndexScan
	{
		private readonly IIndexScan underlyingIndexScan;
		private readonly INormalizer normalizer;

		public NormalizedIndexScan(IIndexScan underlyingIndexScan, INormalizer normalizer)
		{
			this.underlyingIndexScan = underlyingIndexScan;
			this.normalizer = normalizer;
		}

		/// <inheritdoc />
		IReadOnlyCollection<IDocumentLink> IIndexScan.Execute(ITermsIndex index, INode searchExpression)
			=> searchExpression
				.MapTerms(term => term with { Value = normalizer.Normalize(term.Value)})
				.To(expression => underlyingIndexScan.Execute(index, expression));
	}
}