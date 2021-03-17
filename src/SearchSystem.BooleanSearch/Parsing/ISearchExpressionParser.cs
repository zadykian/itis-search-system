namespace SearchSystem.BooleanSearch.Parsing
{
	internal interface ISearchExpressionParser
	{
		/// <summary>
		/// Try parse string <paramref name="booleanSearchRequest"/> to boolean
		/// search expression <see cref="INode"/>. 
		/// </summary>
		/// <param name="booleanSearchRequest">
		/// Raw search request.
		/// </param>
		/// <returns>
		/// Parsing result.
		/// </returns>
		IParseResult Parse(string booleanSearchRequest);
	}
}