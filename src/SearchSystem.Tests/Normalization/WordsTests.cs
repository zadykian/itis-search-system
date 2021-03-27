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
			var (input, result) = testCase;
			var words = input.Words().ToImmutableArray();

			Assert.IsTrue(
				words.SequenceEqual(result),
				$"expected: [{testCase.Result.JoinBy(", ")}], but was: [{words.JoinBy(",")}]");
		}

		/// <summary>
		/// Get test cases for <see cref="SplitStringTest"/>. 
		/// </summary>
		private static IEnumerable<TestCase> TestCases()
		{
			yield break;
		}

		/// <summary>
		/// Line splitting test case.
		/// </summary>
		public record TestCase(string Input, IReadOnlyCollection<string> Result);
	}
}