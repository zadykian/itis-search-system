using System;
using System.Collections.Generic;
using System.Linq;

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

		/// <summary>
		/// Return enumerable which contains elements distinct by <paramref name="selector"/> function result. 
		/// </summary>
		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> selector)
		{
			var hashSet = new HashSet<TKey>();
			return enumerable.Where(value => hashSet.Add(selector(value)));
		}
	}
}