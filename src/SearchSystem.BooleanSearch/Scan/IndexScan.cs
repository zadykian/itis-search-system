using System.Collections.Generic;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.Documents;

namespace SearchSystem.BooleanSearch.Scan
{
	/// <inheritdoc />
	internal class IndexScan : IIndexScan
	{
		/// <inheritdoc />
		IReadOnlyCollection<IDocument> IIndexScan.Execute(IDocumentsIndex index, INode searchExpression)
			=> throw new System.NotImplementedException();
	}
}