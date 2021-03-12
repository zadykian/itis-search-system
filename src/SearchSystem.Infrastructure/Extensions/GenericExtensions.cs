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
	}
}