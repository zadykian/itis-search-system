namespace SearchSystem.BooleanSearch
{
	/// <summary>
	/// Base type for all search expression nodes.
	/// </summary>
	internal interface INode
	{
	}

	/// <summary>
	/// Search expression node which contains word's lemma.
	/// </summary>
	internal record LemmaNode(string Value) : INode;

	/// <summary>
	/// Search expression node which represents negation operator '!'.
	/// </summary>
	internal record NegationNode(INode Node) : INode;

	/// <summary>
	/// Search expression node which represents disjunction binary operator '|'.
	/// </summary>
	internal record DisjunctionNode(INode Left, INode Right) : INode;

	/// <summary>
	/// Search expression node which represents conjunction binary operator '&'.
	/// </summary>
	internal record ConjunctionNode(INode Left, INode Right) : INode;
}