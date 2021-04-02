using System.Collections.Generic;
using NUnit.Framework;
using SearchSystem.Normalization;
using SearchSystem.Normalization.Normalizer;
using SearchSystem.Tests.Base;

namespace SearchSystem.Tests.Normalization
{
	/// <summary>
	/// Text normalization tests.
	/// </summary>
	internal class NormalizerTests : SingleComponentTestFixtureBase<NormalizationAppComponent>
	{
		/// <summary>
		/// Normalize english word. 
		/// </summary>
		[Test]
		[TestCaseSource(nameof(EnglishWordsTestCases))]
		public void NormalizeEnglishWordsTest((string Input, string Expected) testCase)
		{
			var normalizer = GetService<INormalizer>();
			var normalizedWord = normalizer.Normalize(testCase.Input);
			Assert.AreEqual(testCase.Expected, normalizedWord);
		}

		/// <summary>
		/// Test cases for english words normalization test. 
		/// </summary>
		private static IEnumerable<(string, string)> EnglishWordsTestCases()
			=> new[]
			{
				("houses", "house"),
				("Capital", "capital")
			};
	}
}