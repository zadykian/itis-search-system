using System;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.Infrastructure.EnginePhases
{
	public static class Composable
	{
		/// <summary>
		/// Create composition of two functions. 
		/// </summary>
		public static Func<TIn, TOut> Compose<TIn, TInterim, TOut>(
			this Func<TIn, TInterim> leftFunc,
			Func<TInterim, TOut> rightFunc) => input => leftFunc(input).To(rightFunc);

		/// <summary>
		/// Create composition of two functions. 
		/// </summary>
		public static Action<TIn> Compose<TIn, TInterim>(
			this Func<TIn, TInterim> leftFunc,
			Action<TInterim> rightFunc) => input => leftFunc(input).To(rightFunc);
	}
}