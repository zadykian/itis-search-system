using System.Collections.Generic;

namespace SearchSystem.Infrastructure.Documents
{
	/// <inheritdoc cref="IDocument"/>
	public record Document(
		string SubsectionName,
		string Name,
		IReadOnlyCollection<string> Lines) : DocumentLink(SubsectionName, Name), IDocument;
}