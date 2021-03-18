namespace SearchSystem.BooleanSearch
{
	/// <summary>
	/// Base type for all search expression nodes.
	/// </summary>
	internal interface INode
	{
		/// <summary>
		/// Search expression node which contains word's lemma.
		/// </summary>
		public record Lemma(string Value) : INode;

		/// <summary>
		/// Search expression node which represents negation operator '!'.
		/// </summary>
		public record Negation(INode Node) : INode;

		/// <summary>
		/// Search expression node which represents disjunction binary operator '|'.
		/// </summary>
		public record Disjunction(INode Left, INode Right) : INode;

		/// <summary>
		/// Search expression node which represents conjunction binary operator '&'.
		/// </summary>
		public record Conjunction(INode Left, INode Right) : INode;
	}
}