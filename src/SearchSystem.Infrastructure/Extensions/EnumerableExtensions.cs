using System;
using System.Collections.Generic;

namespace SearchSystem.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IEnumerable{T}"/> type.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Perform action <paramref name="action"/> for each item in <paramref name="enumerable"/>. 
		/// </summary>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}
	}
}