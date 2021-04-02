using System.Collections.Generic;

namespace SearchSystem.Infrastructure.Words
{
	/// <summary>
	/// Words extractor.
	/// </summary>
	public interface IWordExtractor
	{
		/// <summary>
		/// Parse <paramref name="textLine"/> to separate words 
		/// </summary>
		IEnumerable<string> Parse(string textLine);
	}
}