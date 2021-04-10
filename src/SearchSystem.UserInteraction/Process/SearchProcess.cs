using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.WebPages;
using SearchSystem.UserInteraction.Result;
using SearchSystem.UserInteraction.UserInterface;

// ReSharper disable TailRecursiveCall
// ReSharper disable BuiltInTypeReferenceStyle
using Request = System.String;

namespace SearchSystem.UserInteraction.Process
{
	/// <inheritdoc />
	internal class SearchProcess : ISearchProcess
	{
		private readonly IUserInterface userInterface;
		private readonly IAppEnvironment<SearchProcess> appEnvironment;

		public SearchProcess(
			IUserInterface userInterface,
			IAppEnvironment<SearchProcess> appEnvironment)
		{
			this.userInterface = userInterface;
			this.appEnvironment = appEnvironment;
		}

		/// <inheritdoc />
		public async Task HandleSearchRequests(Func<Request, ISearchResult> searchCoreFunc)
		{
			userInterface.ShowMessage($"{Environment.NewLine}Enter search expression:");
			var searchRequest = await userInterface.ConsumeInputAsync();
			var stopwatch = Stopwatch.StartNew();

			var resultText = searchCoreFunc(searchRequest) switch
			{
				ISearchResult.Success success => await StringRepresentation(success.Items, stopwatch.Elapsed),
				ISearchResult.Failure failure => failure.ErrorText,
				_ => throw new ArgumentOutOfRangeException(nameof(ISearchResult))
			};

			userInterface.ShowMessage(resultText);
			userInterface.ShowMessage($"{Environment.NewLine}exit? [yes/no]");
			var input = await userInterface.ConsumeInputAsync();

			if (!string.Equals(input, "yes", StringComparison.InvariantCultureIgnoreCase))
			{
				await HandleSearchRequests(searchCoreFunc);
			}
		}

		/// <summary>
		/// Get string representation of found docs list. 
		/// </summary>
		private async Task<string> StringRepresentation(
			IReadOnlyCollection<ISearchResultItem> searchResultItems,
			TimeSpan elapsed)
			=> (await appEnvironment.Storage.LoadAsync(appEnvironment.Storage.Conventions.WebPagesIndex))
				.To(document => new WebPagesIndex(document))
				.SavedPages
				.Join(
					searchResultItems,
					entry => entry.PageId, item => item.PageId,
					(entry, item) => (Page: entry, Result: item))
				.OrderByDescending(tuple => tuple.Result)
				.Select(tuple => tuple
					.Result
					.AdditionalInfo()
					.To(info => string.IsNullOrWhiteSpace(info) ? string.Empty : $" ({info})")
					.To(info => $"{tuple.Page.PageId}. {tuple.Page.PageUri}{info}"))
				.BeginWith($@"Found {searchResultItems.Count} page(s). Elapsed {elapsed:s\.fff}s")
				.JoinBy( Environment.NewLine);
	}
}