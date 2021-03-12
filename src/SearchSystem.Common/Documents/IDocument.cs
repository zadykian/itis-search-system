using System.Collections.Generic;

namespace SearchSystem.Common.Documents
{
	/// <summary>
	/// Text document.
	/// </summary>
	public interface IDocument
	{
		/// <summary>
		/// Document subsection name.
		/// </summary>
		string SubsectionName { get; }
		
		/// <summary>
		/// Document name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Document text lines. 
		/// </summary>
		IReadOnlyCollection<string> Lines { get; }
	}
}