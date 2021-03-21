using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.Documents;

namespace SearchSystem.BooleanSearch.Scan
{
	/// <inheritdoc />
	internal class IndexScan : IIndexScan
	{
		/// <inheritdoc />
		IReadOnlyCollection<IDocumentLink> IIndexScan.Execute(IDocumentsIndex index, INode searchExpression)
			=> RetrieveDocLinks(index, searchExpression);

		private static IImmutableSet<IDocumentLink> RetrieveDocLinks(
			IDocumentsIndex index,
			INode searchExpression)
			=> searchExpression switch
			{
				INode.Term term => index.AllWhichContains(term.Value),
				INode.Not  not  => throw new NotImplementedException(),
				INode.And  and  => throw new NotImplementedException(),
				INode.Or   or   => throw new NotImplementedException(),
				_ => throw new ArgumentOutOfRangeException(nameof(searchExpression))
			};
	}
}