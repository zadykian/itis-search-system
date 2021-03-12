using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;

namespace SearchSystem.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IServiceCollection"/> type.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Add all services from component <typeparamref name="TAppComponent"/>
		/// to collection <paramref name="serviceCollection"/>. 
		/// </summary>
		public static IServiceCollection AddComponent<TAppComponent>(this IServiceCollection serviceCollection)
			where TAppComponent : IAppComponent, new()
		{
			new TAppComponent().RegisterServices(serviceCollection);
			return serviceCollection;
		}
	}
}