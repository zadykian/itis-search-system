using System;

namespace SearchSystem.Infrastructure.Extensions
{
	/// <summary>
	/// Generic extension methods.
	/// </summary>
	public static class GenericExtensions
	{
		/// <summary>
		/// Apply value <paramref name="inputValue"/> to function <paramref name="func"/>. 
		/// </summary>
		public static TOut To<TIn, TOut>(this TIn inputValue, Func<TIn, TOut> func) => func(inputValue);

		/// <summary>
		/// Apply value <paramref name="inputValue"/> to function <paramref name="action"/>. 
		/// </summary>
		public static void To<TIn>(this TIn inputValue, Action<TIn> action) => action(inputValue);

		/// <summary>
		/// Check if <paramref name="itemToCompare"/> belongs
		/// to range [<paramref name="leftBound"/>..<paramref name="rightBound"/>]. 
		/// </summary>
		public static bool Between<T>(this T itemToCompare, T leftBound, T rightBound)
			where T : IComparable<T>
		{
			if (leftBound.CompareTo(rightBound) > 0)
			{
				throw new ArgumentException($"'{leftBound}' must be less or equal to '{rightBound}'.");
			}

			return itemToCompare.CompareTo(leftBound) >= 0 && itemToCompare.CompareTo(rightBound) <= 0;
		}
	}
}