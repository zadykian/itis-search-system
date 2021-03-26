using System.Linq;
using NUnit.Framework;
using SearchSystem.BooleanSearch;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Tests.Base;

namespace SearchSystem.Tests.BooleanSearch
{
	/// <summary>
	/// Tests of search expression Map method.
	/// </summary>
	internal class MapTermsTest : SingleComponentTestFixtureBase<BooleanSearchAppComponent>
	{
		/// <summary>
		/// Map single term.
		/// </summary>
		[Test]
		public void MapSingleTermTest()
		{
			var node = new INode.Term("value");

			const string newValue = "new-value";
			var newNode = node.MapTerms(term => term with { Value = newValue });
			Assert.IsInstanceOf<INode.Term>(newNode);
			Assert.AreEqual(newValue, ((INode.Term) newNode).Value);
		}

		/// <summary>
		/// Map complex search expression with multiple term nodes.
		/// </summary>
		[Test]
		public void MapComplexExpressionTest()
		{
			var expression = new INode.Or(
				new INode.And(
					new INode.Term("str1"),
					new INode.Term("str2")),
				new INode.Not(
					new INode.Term("str3")));

			const string newValue = "new-value";
			var newExpression = expression.MapTerms(term => term with { Value = newValue});
			
			Assert.IsInstanceOf<INode.Or>(newExpression);

			var (left, right) = (INode.Or) newExpression;
			Assert.IsInstanceOf<INode.And>(left);
			Assert.IsInstanceOf<INode.Not>(right);

			var (leftLeft, leftRight) = (INode.And) left;
			Assert.IsInstanceOf<INode.Term>(leftLeft);
			Assert.IsInstanceOf<INode.Term>(leftRight);

			var rightNot = ((INode.Not) right).Node;
			Assert.IsInstanceOf<INode.Term>(rightNot);

			new[] {leftLeft, leftRight, rightNot}
				.Cast<INode.Term>()
				.All(term => term.Value == newValue)
				.To(Assert.IsTrue);
		}
	}
}