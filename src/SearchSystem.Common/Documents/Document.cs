using System.Collections.Generic;

namespace SearchSystem.Common.Documents
{
	/// <inheritdoc cref="IDocument"/>
	public record Document(
		string SubsectionName,
		string Name,
		IReadOnlyCollection<string> Lines) : IDocument;
}