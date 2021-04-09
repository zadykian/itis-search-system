using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.UserInteraction.Process;
using SearchSystem.UserInteraction.UserInterface;

namespace SearchSystem.UserInteraction
{
	/// <inheritdoc />
	public class UserInteractionAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IUserInterface, ConsoleUserInterface>()
				.AddSingleton<ISearchProcess, SearchProcess>();
	}
}