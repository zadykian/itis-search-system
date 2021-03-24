using System;
using System.Threading.Tasks;
using SearchSystem.BooleanSearch.UserInterface;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Normalizer;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc cref="IBooleanSearchEnginePhase" />
	internal class BooleanSearchEnginePhase : EnginePhaseBase<IDocumentsIndex, Unit>, IBooleanSearchEnginePhase
	{
		private readonly IUserInterface userInterface;
		private readonly INormalizer normalizer;

		public BooleanSearchEnginePhase(
			IUserInterface userInterface,
			INormalizer normalizer,
			IAppEnvironment<EnginePhaseBase<IDocumentsIndex, Unit>> appEnvironment) : base(appEnvironment)
		{
			this.userInterface = userInterface;
			this.normalizer = normalizer;
		}

		/// <inheritdoc />
		protected override async Task<Unit> ExecuteAnewAsync(IDocumentsIndex inputData)
		{
			userInterface.ShowMessage("enter search expression:");
			var input = await userInterface.ConsumeInputAsync();
			
			return Unit.Instance;
		}

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync() => Task.FromResult(Unit.Instance);
	}
}