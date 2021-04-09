using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;

// ReSharper disable BuiltInTypeReferenceStyle
using DocLinks = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase" />
	internal class BooleanSearchEnginePhase : TerminatingEnginePhaseBase<ITermsIndex>, ISearchAlgorithmEnginePhase
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
		protected override async Task ExecuteAsync(ITermsIndex inputData)
		{
			userInterface.ShowMessage($"{Environment.NewLine}enter search expression:");
			var searchRequest = await userInterface.ConsumeInputAsync();
			var stopwatch = Stopwatch.StartNew();

			var resultText = expressionParser.Parse(searchRequest) switch
			{
				IParseResult.Success success => await success
					.SearchExpression
					.To(expression =>
					{
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

			if (string.Equals(input, "yes", StringComparison.InvariantCultureIgnoreCase))
			{
				await ExecuteAsync(inputData);
			}
		}
	}
}