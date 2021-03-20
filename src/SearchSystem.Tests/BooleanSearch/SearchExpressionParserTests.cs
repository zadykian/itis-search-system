using System.Collections.Generic;
using System.Linq;
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
		[TestCaseSource(nameof(AllTestCases))]
		public void ParseExpressionTest(TestCase testCase)
		{
			var parser = GetService<ISearchExpressionParser>();
			var parseResult = parser.Parse(testCase.Input);

			Assert.AreEqual(testCase.Result.GetType(), parseResult.GetType());

			if (testCase.Result is not IParseResult.Success success) return;
			var retrievedExpression = ((IParseResult.Success) parseResult).SearchExpression;
			Assert.AreEqual(success.SearchExpression, retrievedExpression);
		}

		/// <summary>
		/// Get all test cases for parsing test <see cref="ParseExpressionTest"/>. 
		/// </summary>
		private static IEnumerable<TestCase> AllTestCases() => ValidParsingTestCases().Concat(InvalidParsingTestCases());

		/// <summary>
		/// Get successful test cases for parsing test <see cref="ParseExpressionTest"/>. 
		/// </summary>
		private static IEnumerable<TestCase> ValidParsingTestCases()
		{
			yield return new("'lemma'", new INode.Word("lemma"));

			yield return new("! 'some-string'",
				new INode.Not(
					new INode.Word("some-string")));

			yield return new("!!'string'",
				new INode.Not(
					new INode.Not(
						new INode.Word("string"))));

			yield return new("'keyboard' & 'trackball'",
				new INode.And(
					new INode.Word("keyboard"),
					new INode.Word("trackball")));

			yield return new("('str')", new INode.Word("str"));

			yield return new("'elephant' | 'hippo'",
				new INode.Or(
					new INode.Word("elephant"),
					new INode.Word("hippo")));

			yield return new("'applicative' & 'functor' | 'monoid'",
				new INode.Or(
					new INode.And(
						new INode.Word("applicative"),
						new INode.Word("functor")),
					new INode.Word("monoid")));

			yield return new("'str0' | 'str1' & 'str2'",
				new INode.Or(
					new INode.Word("str0"),
					new INode.And(
						new INode.Word("str1"),
						new INode.Word("str2"))));

			yield return new("'str0' | 'str1' | 'str2'",
				new INode.Or(
					new INode.Or(
						new INode.Word("str0"),
						new INode.Word("str1")),
					new INode.Word("str2")));

			yield return new("'str0' & 'str1' & 'str2'",
				new INode.And(
					new INode.And(
						new INode.Word("str0"),
						new INode.Word("str1")),
					new INode.Word("str2")));

			yield return new("'str0' & ('str1' | 'str2')",
				new INode.And(
					new INode.Word("str0"),
					new INode.Or(
						new INode.Word("str1"),
						new INode.Word("str2"))));

			yield return new("'str0' & !'str1'",
				new INode.And(
					new INode.Word("str0"),
					new INode.Not(new INode.Word("str1"))));

			yield return new("!'str0' & 'str1'",
				new INode.And(
					new INode.Not(new INode.Word("str0")),
					new INode.Word("str1")));

			yield return new("!('str0' & 'str1')",
				new INode.Not(
					new INode.And(
						new INode.Word("str0"),
						new INode.Word("str1"))));

			yield return new("( (('str0')) & ! 'str1' | ('str2') )",
				new INode.Or(
					new INode.And(
						new INode.Word("str0"),
						new INode.Not(new INode.Word("str1"))),
					new INode.Word("str2")));
		}

		/// <summary>
		/// Get unsuccessful test cases for parsing test <see cref="ParseExpressionTest"/>. 
		/// </summary>
		private static IEnumerable<TestCase> InvalidParsingTestCases()
		{
			yield return new("");

			yield return new("''");

			yield return new("(");

			yield return new("('str0'");

			yield return new("!()");
		}
	}
}