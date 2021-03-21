namespace SearchSystem.BooleanSearch
{
	/// <summary>
	/// Base type for all search expression nodes.
	/// </summary>
	internal interface INode
	{
		/// <summary>
		/// Search expression node which represents terminal symbol - single word.
		/// </summary>
		record Term(string Value) : INode;

		/// <summary>
		/// Search expression node which represents negation operator '!'.
		/// </summary>
		record Not(INode Node) : INode;

		/// <summary>
		/// Search expression node which represents disjunction binary operator '|'.
		/// </summary>
		record Or(INode Left, INode Right) : INode;

		/// <summary>
		/// Search expression node which represents conjunction binary operator '&'.
		/// </summary>
		record And(INode Left, INode Right) : INode;
	}
}