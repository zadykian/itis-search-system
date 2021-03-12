// using System;
// using SearchSystem.Infrastructure.Extensions;
//
// namespace SearchSystem.Infrastructure.EnginePhases
// {
// 	public static class Composable
// 	{
// 		public static Func<TIn, TOut> Compose<TIn, TOut>(Func<TIn, TOut> func) => func;		
//
// 		/// <summary>
// 		/// Create composition of two functions. 
// 		/// </summary>
// 		public static Func<TIn, TOut> Compose<TIn, TInterim, TOut>(
// 			this Func<TIn, TInterim> leftFunc,
// 			Func<TInterim, TOut> rightFunc) => input => leftFunc(input).To(rightFunc);
// 	}
// }