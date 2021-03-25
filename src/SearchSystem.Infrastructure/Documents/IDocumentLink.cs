using System.IO;

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

	/// <summary>
	/// Extension methods for <see cref="IDocumentLink"/> type.
	/// </summary>
	public static class DocumentLinkExtensions
	{
		/// <summary>
		/// Get relative path to file by its link. 
		/// </summary>
		public static string RelativePath(this IDocumentLink documentLink)
			=> Path.Combine(documentLink.SubsectionName, documentLink.Name);

		/// <summary>
		/// Create document from document link. 
		/// </summary>
		public static IDocument ToDocument(this IDocumentLink documentLink, params string[] lines)
			=> new Document(documentLink.SubsectionName, documentLink.Name, lines);
	}
}