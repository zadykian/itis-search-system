using System.Collections.Generic;

namespace SearchSystem.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IEnumerable{T}"/> type.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Join string values <paramref name="stringValues"/> with separator <paramref name="separator"/>. 
		/// </summary>
		public static string JoinBy(this IEnumerable<string> stringValues, string separator)
			=> string.Join(separator, stringValues);

		/// <summary>
		/// Add item <paramref name="newItem"/> at the beginning of <paramref name="enumerable"/>. 
		/// </summary>
		public static IEnumerable<T> BeginWith<T>(this IEnumerable<T> enumerable, T newItem)
		{
			yield return newItem;

			foreach (var item in enumerable)
			{
				yield return item;
			}
		}
	}
}