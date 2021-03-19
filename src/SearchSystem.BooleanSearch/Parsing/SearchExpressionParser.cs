using Sprache;

namespace SearchSystem.BooleanSearch.Parsing
{
	/// <inheritdoc />
	internal class SearchExpressionParser : ISearchExpressionParser
	{
		/// <inheritdoc />
		IParseResult ISearchExpressionParser.Parse(string booleanSearchRequest)
			=> throw new System.NotImplementedException();

		/// <summary>
		/// Definition of search expression grammar.
		/// </summary>
		private static class SearchExpressionGrammar
		{
			private static Parser<INode> ExpressionParser => Parse.Return(new INode.Word(""));

			/// <summary>
			/// Parser of <see cref="INode.Word"/> sub-expressions. 
			/// </summary>
			private static Parser<INode.Word> Word =>
				from openQuote  in Parse.Char(c: '\'').Token()
				from value      in Parse.CharExcept(c: '\'').Many().Text()
				from closeQuote in Parse.Char(c: '\'').Token()
				select new INode.Word(value);

			/// <summary>
			/// Parser of <see cref="INode.Not"/> sub-expressions. 
			/// </summary>
			private static Parser<INode.Not> Not =>
				from negationOperator  in Parse.Char(c: '!').Token()
				from operandExpression in ExpressionParser.Token()
				select new INode.Not(operandExpression);
		}
	}
}