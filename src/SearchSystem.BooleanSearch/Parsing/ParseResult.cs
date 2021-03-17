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
		public record Success(INode SearchExpression) : IParseResult;

		/// <summary>
		/// Unsuccessful parsing result. 
		/// </summary>
		public record Failure(string ErrorText) : IParseResult;
	}
}