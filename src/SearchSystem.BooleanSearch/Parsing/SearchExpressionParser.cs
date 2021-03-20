using SearchSystem.Infrastructure.Extensions;
using Sprache;

namespace SearchSystem.BooleanSearch.Parsing
{
	/// <inheritdoc />
	internal class SearchExpressionParser : ISearchExpressionParser
	{
		/// <inheritdoc />
		IParseResult ISearchExpressionParser.Parse(string booleanSearchRequest)
			=> Grammar
				.ExpressionParser
				.TryParse(booleanSearchRequest)
				.To(ToParseResult);

		/// <summary>
		/// Convert <paramref name="parserResult"/> to <see cref="IParseResult"/> instance. 
		/// </summary>
		private static IParseResult ToParseResult(IResult<INode> parserResult)
			=> parserResult.WasSuccessful
				? new IParseResult.Success(parserResult.Value)
				: new IParseResult.Failure(parserResult.Message);

		/// <summary>
		/// Definition of search expression grammar.
		/// </summary>
		private static class Grammar
		{
			/// <summary>
			/// General search expression parser.
			/// </summary>
			public static Parser<INode> ExpressionParser
				=> And
					.Or(Not)
					.Or(Word);

			private static Parser<INode> Basis => Not.Or(Word);

			/// <summary>
			/// Parser of <see cref="INode.Or"/> sub-expressions. 
			/// </summary>
			private static Parser<INode> Or =>
				Parse
					.Char(c: '|')
					.Token()
					.To(operatorParser => Parse.ChainOperator(
						operatorParser,
						Basis.Or(And),
						(_, left, right) => new INode.Or(left, right)));

			/// <summary>
			/// Parser of <see cref="INode.Word"/> sub-expressions. 
			/// </summary>
			private static Parser<INode> Word =>
				from openQuote  in Parse.Char(c: '\'').Token()
				from value      in Parse.CharExcept(c: '\'').Many().Text()
				from closeQuote in Parse.Char(c: '\'').Token()
				select new INode.Word(value);

			/// <summary>
			/// Parser of <see cref="INode.Not"/> sub-expressions. 
			/// </summary>
			private static Parser<INode> Not =>
				from negationOperator  in Parse.Char(c: '!').Token()
				from operandExpression in ExpressionParser.Token()
				select new INode.Not(operandExpression);

			/// <summary>
			/// Parser of <see cref="INode.And"/> sub-expressions. 
			/// </summary>
			private static Parser<INode> And =>
				from leftOperand  in ExpressionParser
				from andOperator  in Parse.Char(c: '&')
				from rightOperand in ExpressionParser
				select new INode.And(leftOperand, rightOperand);
		}
	}
}