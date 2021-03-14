using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.Infrastructure.Configuration;

namespace SearchSystem.Tests.Base
{
	/// <summary>
	/// Base class for testing services from component <typeparamref name="TAppComponent"/>.
	/// </summary>
	[TestFixture]
	internal abstract class SingleComponentTestFixtureBase<TAppComponent>
		where TAppComponent : IAppComponent, new()
	{
		private readonly IServiceProvider serviceProvider;

		protected SingleComponentTestFixtureBase()
			=> serviceProvider = CreateServiceCollection().BuildServiceProvider();

		/// <summary>
		/// Create collection of services.
		/// </summary>
		private IServiceCollection CreateServiceCollection()
		{
			var serviceCollection = new ServiceCollection();
			new TAppComponent().RegisterServices(serviceCollection);
			ConfigureServices(serviceCollection);
			return serviceCollection;
		}

		/// <summary>
		/// Get registered implementation of service <typeparamref name="TService"/>.
		/// </summary>
		protected TService GetService<TService>()
			where TService : notnull
			=> serviceProvider.GetRequiredService<TService>();

		/// <summary>
		/// Perform additional services configuration. 
		/// </summary>
		private static void ConfigureServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddLogging()
				.AddSingleton<IConfiguration>(_
					=> new ConfigurationBuilder()
						.AddJsonFile("appsettings.json")
						.AddJsonFile("appsettings.local.json", optional: true)
						.Build())
				.AddSingleton<IAppConfiguration, DefaultAppConfiguration>();
	}
}