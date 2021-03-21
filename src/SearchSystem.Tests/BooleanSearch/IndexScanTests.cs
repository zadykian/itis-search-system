using System.Linq;
using NUnit.Framework;
using SearchSystem.BooleanSearch;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Tests.Base;

namespace SearchSystem.Tests.BooleanSearch
{
	/// <summary>
	/// Tests of documents index scanning.
	/// </summary>
	internal class IndexScanTests : SingleComponentTestFixtureBase<BooleanSearchAppComponent>
	{
		/// <summary>
		/// Scan index based on single word search expression.
		/// </summary>
		[Test]
		public void ScanBasedOnWordTest()
		{
			var document = FromLines("str0", "str1", "str2");
			var index = new DocumentsIndex(new[] {document});
			var searchExpression = new INode.Word("str1");

			var indexScan = GetService<IIndexScan>();
			var foundDocs = indexScan.Execute(index, searchExpression);

			Assert.AreEqual(1, foundDocs.Count);
			Assert.AreEqual(document, foundDocs.Single());
		}

		/// <summary>
		/// Create document from array of lines. 
		/// </summary>
		private static IDocument FromLines(params string[] lines) => new Document(string.Empty, "test", lines);
	}
}