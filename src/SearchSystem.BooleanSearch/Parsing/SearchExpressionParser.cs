using Sprache;

namespace SearchSystem.BooleanSearch.Parsing
{
	/// <inheritdoc />
	internal class SearchExpressionParser : ISearchExpressionParser
	{
		/// <inheritdoc />
		IParseResult ISearchExpressionParser.Parse(string booleanSearchRequest)
			=> throw new System.NotImplementedException();

		private static class SearchExpressionGrammar
		{
			private static Parser<INode.Word> Word =>
				from openQuote  in Parse.Char(c: '\'').Token()
				from value      in Parse.CharExcept(c: '\'').Many().Text()
				from closeQuote in Parse.Char(c: '\'').Token()
				select new INode.Word(value);
		}
	}
}