using System.Collections.Generic;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.Documents;

namespace SearchSystem.BooleanSearch.Scan
{
	/// <summary>
	/// Documents index scanner.
	/// </summary>
	internal interface IIndexScan
	{
		/// <summary>
		/// Perform scanning of <paramref name="index"/> based on <paramref name="searchExpression"/>. 
		/// </summary>
		IReadOnlyCollection<IDocumentLink> Execute(IDocumentsIndex index, INode searchExpression);
	}
}