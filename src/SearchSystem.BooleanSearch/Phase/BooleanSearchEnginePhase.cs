using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.UserInteraction.Process;
using SearchSystem.UserInteraction.Result;

// ReSharper disable BuiltInTypeReferenceStyle
using DocLinks = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase" />
	internal class BooleanSearchEnginePhase : TerminatingEnginePhaseBase<ITermsIndex>, ISearchAlgorithmEnginePhase
	{
		private readonly ISearchProcess searchProcess;
		private readonly ISearchExpressionParser expressionParser;
		private readonly IIndexScan indexScan;

		public BooleanSearchEnginePhase(
			ISearchProcess searchProcess,
			ISearchExpressionParser expressionParser,
			IIndexScan indexScan,
			IAppEnvironment<BooleanSearchEnginePhase> appEnvironment) : base(appEnvironment)
		{
			this.searchProcess = searchProcess;
			this.expressionParser = expressionParser;
			this.indexScan = indexScan;
		}

		/// <inheritdoc />
		protected override Task ExecuteAsync(ITermsIndex inputData)
			=> searchProcess.HandleSearchRequests(request =>
				expressionParser.Parse(request) switch
				{
					IParseResult.Success success => success
						.SearchExpression
						.To(expression => indexScan.Execute(inputData, expression))
						.Select(docLink => new DocLinkResultItem(docLink))
						.ToImmutableArray()
						.To(docLinks => new ISearchResult.Success(docLinks)),

					IParseResult.Failure failure => new ISearchResult.Failure(failure.ErrorText),
					_ => throw new ArgumentOutOfRangeException(nameof(IParseResult))
				});
	}
}