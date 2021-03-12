using System;
using System.Threading.Tasks;

namespace SearchSystem.Infrastructure.EnginePhases
{
	public static class Composable
	{
		public static Func<TIn, TOut> Compose<TIn, TOut>(Func<TIn, TOut> func) => func;

		/// <summary>
		/// Create composition of two functions. 
		/// </summary>
		public static Func<TIn, Task<TOut>> Compose<TIn, TInterim, TOut>(
			this Func<TIn, Task<TInterim>> leftFunc,
			Func<TInterim, Task<TOut>> rightFunc)
			=> async input =>
			{
				var leftResult = await leftFunc(input);
				return await rightFunc(leftResult);
			};
	}
}