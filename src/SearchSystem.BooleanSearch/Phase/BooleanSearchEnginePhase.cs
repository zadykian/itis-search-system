using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.BooleanSearch.UserInterface;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Normalizer;

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

		private async Task<string> StringRepresentation(DocLinks foundDocs, TimeSpan elapsed)
		{
			var webPagesDocument = await documentStorage.LoadAsync(documentStorage.Conventions.WebPagesIndex);
		}

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync() => Task.FromResult(Unit.Instance);
	}
}