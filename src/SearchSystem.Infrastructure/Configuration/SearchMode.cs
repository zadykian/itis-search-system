namespace SearchSystem.Infrastructure.Configuration
{
	/// <summary>
	/// System's search mode.
	/// </summary>
	public enum SearchMode : byte
	{
		/// <summary>
		/// Boolean search mode.
		/// </summary>
		Boolean = 1,

		/// <summary>
		/// Vector search mode.
		/// </summary>
		Vector = 2
	}
}