using System.Collections.Generic;

namespace SearchSystem.Infrastructure.Documents
{
	/// <summary>
	/// Text document.
	/// </summary>
	public interface IDocument : IDocumentLink
	{
		/// <summary>
		/// Document text lines. 
		/// </summary>
		IReadOnlyCollection<string> Lines { get; }
	}
}