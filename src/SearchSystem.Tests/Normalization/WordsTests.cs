using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;
using SearchSystem.Infrastructure;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Tests.Base;

namespace SearchSystem.Tests.Normalization
{
	/// <summary>
	/// Tests of splitting line into separate words.
	/// </summary>
	internal class WordsTests : SingleComponentTestFixtureBase<InfrastructureAppComponent>
	{
		/// <summary>
		/// Split string <see cref="TestCase.Input"/> into words <see cref="TestCase.Result"/>.
		/// </summary>
		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void SplitStringTest(TestCase testCase)
		{
			var extractor = GetService<IWordExtractor>();
			var words = extractor.Parse(testCase.Input).ToImmutableArray();

			static string CommaSeparated(IEnumerable<string> strValues)
				=> strValues
					.Select(str => $"'{str}'")
					.JoinBy(", ")
					.To(withCommas => $"[{withCommas}]");

			Assert.IsTrue(
				words.SequenceEqual(testCase.Result),
				$"expected: {CommaSeparated(testCase.Result)}, but was: {CommaSeparated(words)}");
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

			yield return new(
				"ignore-- repeating --- dashes",
				"ignore", "repeating", "dashes");

			yield return new(
				"",
				Array.Empty<string>());
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