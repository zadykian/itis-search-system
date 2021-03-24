using System;
using System.Threading.Tasks;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.BooleanSearch.UserInterface;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Normalizer;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc cref="IBooleanSearchEnginePhase" />
	internal class BooleanSearchEnginePhase : EnginePhaseBase<IDocumentsIndex, Unit>, IBooleanSearchEnginePhase
	{
		private readonly IUserInterface userInterface;
		private readonly ISearchExpressionParser expressionParser;
		private readonly INormalizer normalizer;
		private readonly IIndexScan indexScan;

		public BooleanSearchEnginePhase(
			IUserInterface userInterface,
			ISearchExpressionParser expressionParser,
			INormalizer normalizer,
			IIndexScan indexScan,
			IAppEnvironment<EnginePhaseBase<IDocumentsIndex, Unit>> appEnvironment) : base(appEnvironment)
		{
			this.userInterface = userInterface;
			this.expressionParser = expressionParser;
			this.normalizer = normalizer;
			this.indexScan = indexScan;
		}

		/// <inheritdoc />
		protected override async Task<Unit> ExecuteAnewAsync(IDocumentsIndex inputData)
		{
			userInterface.ShowMessage("enter search expression:");
			var input = await userInterface.ConsumeInputAsync();

			if (string.Equals(input, "exit", StringComparison.InvariantCultureIgnoreCase))
			{
				return Unit.Instance;
			}

			var parseResult = expressionParser.Parse(input);

			if (parseResult is IParseResult.Failure failure)
			{
				userInterface.ShowMessage(failure.ErrorText);
			}

			var success = (IParseResult.Success) parseResult;

			success
				.SearchExpression
				.MapTerms(term => term with { Value = normalizer.Normalize(term.Value)})
				.To(normalized => indexScan.Execute(inputData, normalized))
				

			return Unit.Instance;
		}

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync() => Task.FromResult(Unit.Instance);
	}
}