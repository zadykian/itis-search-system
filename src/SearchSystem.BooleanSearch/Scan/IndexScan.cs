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

		/// <summary>
		/// Retrieve all links from <paramref name="index"/> which
		/// satisfy expression <paramref name="searchExpression"/>. 
		/// </summary>
		private static IImmutableSet<IDocumentLink> RetrieveDocLinks(
			IDocumentsIndex index,
			INode searchExpression)
			=> searchExpression switch
			{
				INode.Term term => index.AllWhichContains(term.Value),
				INode.Not  not  => index.AllDocuments().Except(RetrieveDocLinks(index, not.Node)),
				INode.Or   or   => RetrieveDocLinks(index, or.Left).Union(RetrieveDocLinks(index, or.Right)),
				INode.And  and  => RetrieveDocLinks(index, and.Left).Intersect(RetrieveDocLinks(index, and.Right)),
				_ => throw new ArgumentOutOfRangeException(nameof(searchExpression), searchExpression, string.Empty)
			};
	}
}