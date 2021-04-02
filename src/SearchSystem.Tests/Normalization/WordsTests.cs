using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.Tests.Normalization
{
	/// <summary>
	/// Tests of splitting line into separate words.
	/// </summary>
	[TestFixture]
	internal class WordsTests
	{
		/// <summary>
		/// Split string <see cref="TestCase.Input"/> into words <see cref="TestCase.Result"/>.
		/// </summary>
		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void SplitStringTest(TestCase testCase)
		{
			var words = testCase.Input.Words().ToImmutableArray();

			Assert.IsTrue(
				words.SequenceEqual(testCase.Result),
				$"expected: [{testCase.Result.JoinBy(", ")}], but was: [{words.JoinBy(", ")}]");
		}

		/// <summary>
		/// Get test cases for <see cref="SplitStringTest"/>. 
		/// </summary>
		private static IEnumerable<TestCase> TestCases()
		{
			yield return new(
				"this is a very simple sentence.",
				"this", "is", "a", "very", "simple", "sentence");

			yield return new(
				"to-do: correctly parse sequence of chars!",
				"to-do", "correctly", "parse", "sequence", "of", "chars");

			yield return new(
				"All digits should 2 be ignored1",
				"All", "digits", "should", "be", "ignored");
		}

		/// <summary>
		/// Line splitting test case.
		/// </summary>
		public readonly struct TestCase
		{
			public TestCase(string input, params string[] words)
			{
				Input = input;
				Result = words;
			}

			/// <summary>
			/// Test input.
			/// </summary>
			public string Input { get; }

			/// <summary>
			/// Test expected result.
			/// </summary>
			public IReadOnlyCollection<string> Result { get; }
		}
	}
}