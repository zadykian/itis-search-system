namespace SearchSystem.Infrastructure.Documents
{
	/// <summary>
	/// Document link, by which document can be identified.
	/// </summary>
	public interface IDocumentLink
	{
		/// <summary>
		/// Document subsection name.
		/// </summary>
		string SubsectionName { get; }

		/// <summary>
		/// Document name.
		/// </summary>
		string Name { get; }
	}
}