using SearchSystem.Indexing.Index;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

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
		Docs Execute(IDocumentsIndex index, INode searchExpression);
	}
}