using System.Collections.Generic;
using NUnit.Framework;
using SearchSystem.BooleanSearch;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.Tests.Base;

namespace SearchSystem.Tests.BooleanSearch
{
	/// <summary>
	/// Tests of raw search expression parsing.
	/// </summary>
	internal class SearchExpressionParserTests : SingleComponentTestFixtureBase<BooleanSearchAppComponent>
	{
		/// <summary>
		/// Parse raw input and return parsing result. 
		/// </summary>
		[Test]
		[TestCaseSource(nameof(ParsingTestCases))]
		public void ParseExpressionTest(TestCase testCase)
		{
			var parser = GetService<ISearchExpressionParser>();
			var parseResult = parser.Parse(testCase.Input);

			Assert.AreEqual(testCase.GetType(), parseResult.GetType());

			if (testCase.Result is not IParseResult.Success success) return;
			var retrievedExpression = ((IParseResult.Success) parseResult).SearchExpression;
			Assert.AreEqual(success.SearchExpression, retrievedExpression);
		}

		/// <summary>
		/// Get test cases for parsing test <see cref="ParseExpressionTest"/>. 
		/// </summary>
		private static IEnumerable<TestCase> ParsingTestCases()
		{
			yield return new("lemma", new INode.Word("lemma"));
			yield return new("(");
		}
	}
}