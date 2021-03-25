using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Normalizer;
using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Normalization.Phase
{
	/// <inheritdoc cref="INormalizationEnginePhase"/>
	internal class NormalizationEnginePhase : DocumentsOutputPhaseBase<Docs>, INormalizationEnginePhase
	{
		private readonly INormalizer normalizer;

		public NormalizationEnginePhase(
			INormalizer normalizer,
			IAppEnvironment<NormalizationEnginePhase> appEnvironment) : base(appEnvironment)
			=> this.normalizer = normalizer;

		/// <inheritdoc />
		protected override async Task<Docs> ExecuteAnewAsync(Docs inputData)
			=> await inputData
				.Select(NormalizeDocument)
				.ToAsyncEnumerable()
				.SelectAwait(async normalizedDocument =>
				{
					await AppEnvironment.Storage.SaveOrAppendAsync(normalizedDocument);
					return normalizedDocument;
				})
				.ToArrayAsync();

		/// <summary>
		/// Perform document normalization. 
		/// </summary>
		private IDocument NormalizeDocument(IDocument document)
			=> document
				.Lines
				.Select(documentLine => documentLine
					.Words()
					.Select(normalizer.Normalize)
					.JoinBy(" "))
				.Where(documentLine => !string.IsNullOrWhiteSpace(documentLine))
				.ToImmutableArray()
				.To(normalizedLines => new Document(ComponentName, document.Name, normalizedLines));
	}
}