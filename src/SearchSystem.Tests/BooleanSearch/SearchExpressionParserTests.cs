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
			yield return new("'lemma'", new INode.Term("lemma"));

			yield return new("! 'some-string'",
				new INode.Not(
					new INode.Term("some-string")));

			yield return new("!!'string'",
				new INode.Not(
					new INode.Not(
						new INode.Term("string"))));

			yield return new("'keyboard' & 'trackball'",
				new INode.And(
					new INode.Term("keyboard"),
					new INode.Term("trackball")));

			yield return new("('str')", new INode.Term("str"));

			yield return new("'elephant' | 'hippo'",
				new INode.Or(
					new INode.Term("elephant"),
					new INode.Term("hippo")));

			yield return new("'applicative' & 'functor' | 'monoid'",
				new INode.Or(
					new INode.And(
						new INode.Term("applicative"),
						new INode.Term("functor")),
					new INode.Term("monoid")));

			yield return new("'str0' | 'str1' & 'str2'",
				new INode.Or(
					new INode.Term("str0"),
					new INode.And(
						new INode.Term("str1"),
						new INode.Term("str2"))));

			yield return new("'str0' | 'str1' | 'str2'",
				new INode.Or(
					new INode.Or(
						new INode.Term("str0"),
						new INode.Term("str1")),
					new INode.Term("str2")));

			yield return new("'str0' & 'str1' & 'str2'",
				new INode.And(
					new INode.And(
						new INode.Term("str0"),
						new INode.Term("str1")),
					new INode.Term("str2")));

			yield return new("'str0' & ('str1' | 'str2')",
				new INode.And(
					new INode.Term("str0"),
					new INode.Or(
						new INode.Term("str1"),
						new INode.Term("str2"))));

			yield return new("'str0' & !'str1'",
				new INode.And(
					new INode.Term("str0"),
					new INode.Not(new INode.Term("str1"))));

			yield return new("!'str0' & 'str1'",
				new INode.And(
					new INode.Not(new INode.Term("str0")),
					new INode.Term("str1")));

			yield return new("!('str0' & 'str1')",
				new INode.Not(
					new INode.And(
						new INode.Term("str0"),
						new INode.Term("str1"))));

			yield return new("( (('str0')) & ! 'str1' | ('str2') )",
				new INode.Or(
					new INode.And(
						new INode.Term("str0"),
						new INode.Not(new INode.Term("str1"))),
					new INode.Term("str2")));
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

		/// <summary>
		/// Parsing test case.
		/// </summary>
		internal readonly struct TestCase
		{
			/// <summary>
			/// Unsuccessful test case ctor. 
			/// </summary>
			public TestCase(string input)
			{
				Input = input;
				Result = new IParseResult.Failure(string.Empty);
			}

			/// <summary>
			/// Successful test case ctor. 
			/// </summary>
			public TestCase(string input, INode searchExpression)
			{
				Input = input;
				Result = new IParseResult.Success(searchExpression);
			}

			/// <summary>
			/// Parser input.
			/// </summary>
			public string Input { get; }

			/// <summary>
			/// Parsing result.
			/// </summary>
			public IParseResult Result { get; }

			/// <inheritdoc />
			public override string ToString() => Input;
		}
	}
}