using System.Collections.Generic;

namespace SearchSystem.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="string"/> type.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Split <paramref name="text"/> to words. 
		/// </summary>
		public static IEnumerable<string> Lines(this string text) => text.Split(separator: '\n');

		/// <summary>
		/// Split <paramref name="textLine"/> to words. 
		/// </summary>
		public static IEnumerable<string> Words(this string textLine) => textLine.Split(' ', '\t', '\n');
	}
}