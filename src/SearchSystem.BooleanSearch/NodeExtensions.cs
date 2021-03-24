using System;

namespace SearchSystem.BooleanSearch
{
	/// <summary>
	/// Extension methods for <see cref="INode"/> type.
	/// </summary>
	internal static class NodeExtensions
	{
		/// <summary>
		/// Map expression's term nodes and at the same time save parent nodes' structure. 
		/// </summary>
		public static INode MapTerms(this INode root, Func<INode.Term, INode> selector)
			=> root switch
			{
				INode.Term term => selector(term),
				INode.Not  not  => not with { Node = not.Node.MapTerms(selector) },
				INode.Or   or   => or  with { Left = or.Left.MapTerms(selector),  Right = or.Right.MapTerms(selector)  },
				INode.And  and  => and with { Left = and.Left.MapTerms(selector), Right = and.Right.MapTerms(selector) },
				_ => throw new ArgumentOutOfRangeException(nameof(root), root, string.Empty)
			};
	}
}