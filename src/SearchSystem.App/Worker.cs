using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SearchSystem.App
{
	/// <inheritdoc />
	internal class Worker : BackgroundService
	{
		/// <inheritdoc />
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await Task.CompletedTask;
			// todo
		}
	}
}