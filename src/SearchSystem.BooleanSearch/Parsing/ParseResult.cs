namespace SearchSystem.BooleanSearch.Parsing
{
	/// <summary>
	/// Search query parsing result.
	/// </summary>
	internal interface IParseResult
	{
		/// <summary>
		/// Successful parsing result.
		/// </summary>
		record Success(INode SearchExpression) : IParseResult;

		/// <summary>
		/// Unsuccessful parsing result. 
		/// </summary>
		record Failure(string ErrorText) : IParseResult;
	}
}