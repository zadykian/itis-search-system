using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.BooleanSearch.UserInterface;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Infrastructure.WebPages;

// ReSharper disable BuiltInTypeReferenceStyle
using PageId = System.UInt32;
using DocLinks = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase" />
	internal class BooleanSearchEnginePhase : EnginePhaseBase<ITermsIndex, Unit>, ISearchAlgorithmEnginePhase
	{
		private readonly IUserInterface userInterface;
		private readonly ISearchExpressionParser expressionParser;
		private readonly IIndexScan indexScan;

		public BooleanSearchEnginePhase(
			IUserInterface userInterface,
			ISearchExpressionParser expressionParser,
			IIndexScan indexScan,
			IAppEnvironment<BooleanSearchEnginePhase> appEnvironment) : base(appEnvironment)
		{
			this.userInterface = userInterface;
			this.expressionParser = expressionParser;
			this.indexScan = indexScan;
		}

		/// <inheritdoc />
		protected override async Task<Unit> ExecuteAnewAsync(ITermsIndex inputData)
		{
			userInterface.ShowMessage($"{Environment.NewLine}enter search expression:");
			var searchRequest = await userInterface.ConsumeInputAsync();

			var resultText = expressionParser.Parse(searchRequest) switch
			{
				IParseResult.Success success => await success
					.SearchExpression
					.To(expression =>
					{
						var stopwatch = Stopwatch.StartNew();
						var result = indexScan.Execute(inputData, expression);
						return (FoundDocs: result, stopwatch.Elapsed);
					})
					.To(tuple => StringRepresentation(tuple.FoundDocs, tuple.Elapsed)),

				IParseResult.Failure failure => failure.ErrorText,
				_ => throw new ArgumentOutOfRangeException(nameof(IParseResult))
			};

			userInterface.ShowMessage(resultText);
			userInterface.ShowMessage($"{Environment.NewLine}exit? [yes/no]");
			var input = await userInterface.ConsumeInputAsync();

			return string.Equals(input, "yes", StringComparison.InvariantCultureIgnoreCase)
				? Unit.Instance
				: await ExecuteAnewAsync(inputData);
		}

		/// <summary>
		/// Get string representation of found docs list. 
		/// </summary>
		private async Task<string> StringRepresentation(DocLinks foundDocs, TimeSpan elapsed)
		{
			var webPagesDocument = await AppEnvironment.Storage.LoadAsync(AppEnvironment.Storage.Conventions.WebPagesIndex);

			var foundPageIds = foundDocs
				.Select(link => Path.GetFileNameWithoutExtension(link.Name))
				.Select(PageId.Parse)
				.ToImmutableArray();

			return new WebPagesIndex(webPagesDocument)
				.SavedPages
				.Where(tuple => foundPageIds.Contains(tuple.PageId))
				.OrderBy(tuple => tuple.PageId)
				.Select(tuple => $"{tuple.PageId}. {tuple.PageUri}")
				.BeginWith($@"Found {foundDocs.Count} page(s). Elapsed {elapsed:s\.fff}s")
				.JoinBy(Environment.NewLine);
		}

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync() => Task.FromResult(Unit.Instance);
	}
}