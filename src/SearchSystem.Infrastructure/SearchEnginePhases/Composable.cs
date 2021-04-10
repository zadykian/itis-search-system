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
		public static Func<Unit, TOut> Add<TOut>(Func<Unit, TOut> func) => func;

		/// <summary>
		/// Create composition of two async functions. 
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