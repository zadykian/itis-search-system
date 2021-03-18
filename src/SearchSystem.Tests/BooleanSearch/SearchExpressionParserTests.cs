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

			yield return new("elephant | hippo",
				new INode.Or(
					new INode.Word("elephant"),
					new INode.Word("hippo")));

			yield return new("keyboard & trackball",
				new INode.And(
					new INode.Word("keyboard"),
					new INode.Word("trackball")));

			yield return new("applicative & functor | monoid",
				new INode.Or(
					new INode.And(
						new INode.Word("applicative"),
						new INode.Word("functor")),
					new INode.Word("monoid")));

			yield return new("str0 | str1 | str2",
				new INode.Or(
					new INode.Word("str0"),
					new INode.Or(
						new INode.Word("str1"),
						new INode.Word("str2"))));

			yield return new("str0 & str1 & str2",
				new INode.And(
					new INode.Word("str0"),
					new INode.And(
						new INode.Word("str1"),
						new INode.Word("str2"))));

			yield return new("hammer & (functor | monoid)",
				new INode.And(
					new INode.Word("hammer"),
					new INode.Or(
						new INode.Word("functor"),
						new INode.Word("monoid"))));
		}
	}
}