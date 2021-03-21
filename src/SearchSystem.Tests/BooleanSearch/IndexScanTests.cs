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
		/// Scan index based on single term search expression.
		/// </summary>
		[Test]
		public void ScanBasedOnSingleTermTest()
		{
			var document = FromLines(0, "str0", "str1", "str2");
			var index = new DocumentsIndex(new[] {document});
			var searchExpression = new INode.Term("str1");

			var indexScan = GetService<IIndexScan>();
			var foundDocs = indexScan.Execute(index, searchExpression);

			Assert.AreEqual(1, foundDocs.Count);
			Assert.AreEqual(document.Name, foundDocs.Single().Name);
		}

		/// <summary>
		/// Scan index based on conjunction expression with negation.
		/// </summary>
		[Test]
		public void ScanBasedOnConjunctionAndNegationTest()
		{
			var whichSatisfies = FromLines(1, "str1", "str2");
			
			var index = new DocumentsIndex(new[]
			{
				FromLines(0, "str0", "str1"),
				whichSatisfies,
				FromLines(2, "str1", "str2", "str3")
			});

			var searchExpression = new INode.And(
				new INode.And(
					new INode.Term("str1"),
					new INode.Term("str2")),
				new INode.Not(
					new INode.Term("str3")));

			var indexScan = GetService<IIndexScan>();
			var foundDocs = indexScan.Execute(index, searchExpression);

			Assert.AreEqual(1, foundDocs.Count);
			Assert.AreEqual(whichSatisfies.Name, foundDocs.Single().Name);
		}

		/// <summary>
		/// Create document from array of lines. 
		/// </summary>
		private static IDocument FromLines(int index, params string[] lines) => new Document(string.Empty, $"test-{index}", lines);
	}
}