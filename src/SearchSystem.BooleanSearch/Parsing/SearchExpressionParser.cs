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
			public static Parser<INode> ExpressionParser => Or;

			/// <summary>
			/// Parser of <see cref="INode.Or"/> sub-expressions.
			/// </summary>
			/// <remarks>
			/// Disjunction operator has lowest priority, so
			/// node <see cref="INode.Or"/> always must be the root of search expression tree,
			/// except cases with parentheses.
			/// </remarks>
			private static Parser<INode> Or
				=> Parse
					.Char('|')
					.To(operatorParser => Parse.ChainOperator(
						operatorParser,
						And,
						(_, left, right) => new INode.Or(left, right)));

			/// <summary>
			/// Parser of <see cref="INode.And"/> sub-expressions.
			/// </summary>
			private static Parser<INode> And
				=> Parse
					.Char('&')
					.To(operatorParser => Parse.ChainOperator(
						operatorParser,
						HighestPriority,
						(_, left, right) => new INode.And(left, right)));

			/// <summary>
			/// <para>
			/// Parser of sub-expressions with highest priority:
			/// </para>
			/// <para>
			/// 1. Terminals <see cref="INode.Term"/>;
			/// 2. Negation operators <see cref="INode.Not"/>;
			/// 3. Sub-expressions in parentheses.
			/// </para>
			/// </summary>
			private static Parser<INode> HighestPriority
				=> Term
					.Or(Not)
					.Or(Parentheses);

			/// <summary>
			/// Parser of <see cref="INode.Term"/> sub-expressions.
			/// </summary>
			private static Parser<INode> Term =>
				from openQuote  in Parse.Char('\'')
				from value      in Parse.CharExcept('\'').AtLeastOnce().Text()
				from closeQuote in Parse.Char('\'')
				select new INode.Term(value);

			/// <summary>
			/// Parser of <see cref="INode.Not"/> sub-expressions.
			/// </summary>
			/// <remarks>
			/// Any valid expression with equal or higher priority
			/// can be negation operator's operand, including other negation expression.
			/// </remarks>
			private static Parser<INode> Not =>
				from negationOperator  in Parse.Char('!')
				from operandExpression in HighestPriority
				select new INode.Not(operandExpression);

			/// <summary>
			/// Parser of bracketed sub-expressions.
			/// </summary>
			/// <remarks>
			/// Parentheses can contain any valid expression, including
			/// other parentheses.
			/// </remarks>
			private static Parser<INode> Parentheses =>
				from leftParenthesis  in Parse.Char('(')
				from subExpression    in ExpressionParser
				from rightParenthesis in Parse.Char(')')
				select subExpression;
		}
	}
}