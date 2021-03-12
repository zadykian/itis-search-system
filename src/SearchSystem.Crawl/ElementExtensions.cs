using System.Linq;
using AngleSharp.Dom;

namespace SearchSystem.Crawl
{
	/// <summary>
	/// Extension methods for <see cref="IElement"/> type;
	/// </summary>
	internal static class ElementExtensions
	{
		/// <summary>
		/// Determine if <paramref name="element"/> has at most one child.
		/// </summary>
		public static bool HasZeroOrOneChild(this IElement element)
		{
			while (element.ChildElementCount == 1)
			{
				element = element.Children.Single();
			}

			return element.ChildElementCount == 0;
		}

		/// <summary>
		/// Flatten <paramref name="element"/> if it's represented
		/// as chain of elements. 
		/// </summary>
		public static IElement Flatten(this IElement element)
		{
			while (element.ChildElementCount == 1)
			{
				element = element.Children.First();
			}

			return element;
		}
	}
}