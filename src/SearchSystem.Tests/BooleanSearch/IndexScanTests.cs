using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SearchSystem.BooleanSearch;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Words;
using SearchSystem.Normalization;
using SearchSystem.Tests.Base;

namespace SearchSystem.Tests.BooleanSearch
{
	/// <summary>
	/// Tests of documents index scanning.
	/// </summary>
	internal class IndexScanTests : SingleComponentTestFixtureBase<BooleanSearchAppComponent>
	{
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			IAppComponent normalizationAppComponent = new NormalizationAppComponent();
			normalizationAppComponent.RegisterServices(serviceCollection);
		}

		/// <summary>
		/// Scan index based on single term search expression.
		/// </summary>
		[Test]
		public void ScanBasedOnSingleTermTest()
		{
			var document = FromLines(0, "first-string", "second-string", "third-string");
			var index = new TermsIndex(new[] {document}, new SimpleWordExtractor());
			var searchExpression = new INode.Term("second-string");

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
			var whichSatisfies = FromLines(1, "second-string", "third-string");

			var index = new TermsIndex(new[]
			{
				FromLines(0, "first-string", "second-string"),
				whichSatisfies,
				FromLines(2, "second-string", "third-string", "forth-string")
			}, new SimpleWordExtractor());

			var searchExpression = new INode.And(
				new INode.And(
					new INode.Term("second-string"),
					new INode.Term("third-string")),
				new INode.Not(
					new INode.Term("forth-string")));

			var indexScan = GetService<IIndexScan>();
			var foundDocs = indexScan.Execute(index, searchExpression);

			Assert.AreEqual(1, foundDocs.Count);
			Assert.AreEqual(whichSatisfies.Name, foundDocs.Single().Name);
		}

		/// <summary>
		/// Create document from array of lines. 
		/// </summary>
		private static IDocument FromLines(int index, params string[] lines) => new Document(string.Empty, $"test-{index}", lines);

		/// <inheritdoc />
		private sealed class SimpleWordExtractor : IWordExtractor
		{
			/// <inheritdoc />
			IEnumerable<string> IWordExtractor.Parse(string textLine) => textLine.Split(' ');
		}
	}
}