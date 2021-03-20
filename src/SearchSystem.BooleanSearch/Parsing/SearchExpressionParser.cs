using System.Text.RegularExpressions;
using SearchSystem.Infrastructure.Extensions;
using Sprache;

namespace SearchSystem.BooleanSearch.Parsing
{
	/// <inheritdoc />
	internal class SearchExpressionParser : ISearchExpressionParser
	{
		/// <inheritdoc />
		IParseResult ISearchExpressionParser.Parse(string booleanSearchRequest)
			=> Regex
				.Replace(booleanSearchRequest, @"\s+", string.Empty)
				.To(Grammar.ExpressionParser.TryParse)
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
			/// <remarks>
			/// Disjunction operator has lowest priority, so
			/// node <see cref="INode.Or"/> always must be the root of search expression tree.
			/// </remarks>
			public static Parser<INode> ExpressionParser => Or.End();

			/// <summary>
			/// Parser of <see cref="INode.Or"/> sub-expressions.
			/// </summary>
			private static Parser<INode> Or
				=> Parse
					.Char('|')
					.To(operatorParser => Parse.ChainOperator(
						operatorParser,
						And.Or(Basis),
						(_, left, right) => new INode.Or(left, right)));

			/// <summary>
			/// Parser of <see cref="INode.And"/> sub-expressions.
			/// </summary>
			private static Parser<INode> And
				=> Parse
					.Char('&')
					.To(operatorParser => Parse.ChainOperator(
						operatorParser,
						Basis,
						(_, left, right) => new INode.And(left, right)));

			private static Parser<INode> Basis
				=> Word
					.Or(Not)
					.Or(Parentheses);

			/// <summary>
			/// Parser of <see cref="INode.Word"/> sub-expressions.
			/// </summary>
			private static Parser<INode> Word =>
				from openQuote  in Parse.Char('\'')
				from value      in Parse.CharExcept('\'').AtLeastOnce().Text()
				from closeQuote in Parse.Char('\'')
				select new INode.Word(value);

			/// <summary>
			/// Parser of <see cref="INode.Not"/> sub-expressions.
			/// </summary>
			private static Parser<INode> Not =>
				from negationOperator  in Parse.Char('!')
				from operandExpression in ExpressionParser
				select new INode.Not(operandExpression);

			/// <summary>
			/// Parser of bracketed sub-expressions.
			/// </summary>
			private static Parser<INode> Parentheses =>
				from leftParenthesis  in Parse.Char('(')
				from subExpression    in ExpressionParser
				from rightParenthesis in Parse.Char(')')
				select subExpression;
		}
	}
}