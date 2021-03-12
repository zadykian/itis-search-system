using System.Collections.Generic;

namespace SearchSystem.Common.Documents
{
	/// <summary>
	/// Text document.
	/// </summary>
	public interface IDocument
	{
		/// <summary>
		/// Document path relative to current directory.
		/// </summary>
		string RelativePath { get; }

		/// <summary>
		/// Document text lines. 
		/// </summary>
		IReadOnlyCollection<string> Lines { get; }
	}
}