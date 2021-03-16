using System;
using System.Threading.Tasks;

namespace SearchSystem.Infrastructure.SearchEnginePhases
{
	/// <summary>
	/// Methods for function composition.
	/// </summary>
	public static class Composable
	{
		/// <summary>
		/// Begin function composition chain. 
		/// </summary>
		public static Func<TIn, TOut> Add<TIn, TOut>(Func<TIn, TOut> func) => func;

		/// <summary>
		/// Create composition of two functions. 
		/// </summary>
		public static Func<TIn, Task<TOut>> Add<TIn, TInterim, TOut>(
			this Func<TIn, Task<TInterim>> leftFunc,
			Func<TInterim, Task<TOut>> rightFunc)
			=> async input =>
			{
				var leftResult = await leftFunc(input);
				return await rightFunc(leftResult);
			};
	}
}