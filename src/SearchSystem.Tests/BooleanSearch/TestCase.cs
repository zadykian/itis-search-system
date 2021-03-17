using SearchSystem.BooleanSearch;
using SearchSystem.BooleanSearch.Parsing;

namespace SearchSystem.Tests.BooleanSearch
{
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
	}
}