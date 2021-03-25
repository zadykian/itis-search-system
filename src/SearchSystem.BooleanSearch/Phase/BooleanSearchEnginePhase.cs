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
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Infrastructure.WebPages;
using SearchSystem.Normalization.Normalizer;

// ReSharper disable BuiltInTypeReferenceStyle
using PageId = System.UInt32;
using DocLinks = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc cref="IBooleanSearchEnginePhase" />
	internal class BooleanSearchEnginePhase : EnginePhaseBase<ITermsIndex, Unit>, IBooleanSearchEnginePhase
	{
		private readonly IUserInterface userInterface;
		private readonly ISearchExpressionParser expressionParser;
		private readonly INormalizer normalizer;
		private readonly IIndexScan indexScan;
		private readonly IDocumentStorage documentStorage;

		public BooleanSearchEnginePhase(
			IUserInterface userInterface,
			ISearchExpressionParser expressionParser,
			INormalizer normalizer,
			IIndexScan indexScan,
			IDocumentStorage documentStorage,
			IAppEnvironment<EnginePhaseBase<ITermsIndex, Unit>> appEnvironment) : base(appEnvironment)
		{
			this.userInterface = userInterface;
			this.expressionParser = expressionParser;
			this.normalizer = normalizer;
			this.indexScan = indexScan;
			this.documentStorage = documentStorage;
		}

		/// <inheritdoc />
		protected override async Task<Unit> ExecuteAnewAsync(ITermsIndex inputData)
		{
			userInterface.ShowMessage("enter search expression:");
			var searchRequest = await userInterface.ConsumeInputAsync();

			var resultText = expressionParser.Parse(searchRequest) switch
			{
				IParseResult.Success success => await success
					.SearchExpression
					.MapTerms(term => term with { Value = normalizer.Normalize(term.Value)})
					.To(normalized =>
					{
						var stopwatch = Stopwatch.StartNew();
						var result = indexScan.Execute(inputData, normalized);
						return (FoundDocs: result, stopwatch.Elapsed);
					})
					.To(tuple => StringRepresentation(tuple.FoundDocs, tuple.Elapsed)),

				IParseResult.Failure failure => failure.ErrorText,
				_ => throw new ArgumentOutOfRangeException(nameof(IParseResult))
			};

			userInterface.ShowMessage(resultText);
			userInterface.ShowMessage("exit?");
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
			var webPagesDocument = await documentStorage.LoadAsync(documentStorage.Conventions.WebPagesIndex);

			var foundPageIds = foundDocs
				.Select(link => Path.GetFileNameWithoutExtension(link.Name))
				.Select(PageId.Parse)
				.ToImmutableArray();

			return new WebPagesIndex(webPagesDocument)
				.SavedPages
				.Where(tuple => foundPageIds.Contains(tuple.PageId))
				.OrderBy(tuple => tuple.PageId)
				.Select(tuple => $"{tuple.PageId}. {tuple.PageUri}")
				.BeginWith($"Found {foundDocs.Count} page(s). Elapsed {elapsed:s.fff}s")
				.JoinBy(Environment.NewLine);
		}

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync() => Task.FromResult(Unit.Instance);
	}
}