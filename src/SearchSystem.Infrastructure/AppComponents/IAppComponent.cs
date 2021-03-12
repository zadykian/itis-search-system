using Microsoft.Extensions.DependencyInjection;

namespace SearchSystem.Infrastructure.AppComponents
{
	/// <summary>
	/// Component which contains application services.
	/// </summary>
	public interface IAppComponent
	{
		/// <summary>
		/// Install component's services.
		/// </summary>
		/// <param name="serviceCollection">
		/// Collection of application services.
		/// </param>
		void RegisterServices(IServiceCollection serviceCollection);
	}
}