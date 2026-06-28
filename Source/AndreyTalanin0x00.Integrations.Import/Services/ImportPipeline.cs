using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Exceptions.Factories;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

// Disable the IDE0032 (Use auto property) notification to preserve easily recognizable 'injected services' field pattern.
#pragma warning disable IDE0032

// Disable the IDE0305 (Simplify collection initialization) notification to preserve the LINQ call chain.
#pragma warning disable IDE0305

namespace AndreyTalanin0x00.Integrations.Import.Services;

public class ImportPipeline<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    IImportPipeline<TImportRequest, TImportResponse>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    private readonly IImportReader<TImportRequest, TImportResponse> m_importReader;
    private readonly IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse> m_importPipelineChannelKeyResolver;
    private readonly List<IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>> m_importPipelineChannels;
    private readonly Dictionary<ImportPipelineChannelKey, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>> m_importPipelineChannelsDictionary;
    private readonly IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> m_importNormalizer;
    private readonly IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> m_importValidator;
    private readonly IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> m_importProcessor;

    public ImportPipeline(
        IImportReader<TImportRequest, TImportResponse> importReader,
        IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse> importPipelineChannelKeyResolver,
        IEnumerable<IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>> importPipelineChannels,
        IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importValidator,
        IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importNormalizer,
        IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importProcessor)
    {
        m_importReader = importReader;
        m_importPipelineChannelKeyResolver = importPipelineChannelKeyResolver;
        m_importPipelineChannels = [.. importPipelineChannels];
        m_importPipelineChannelsDictionary = importPipelineChannels.ToDictionary(importPipelineChannel => importPipelineChannel.Key);
        m_importNormalizer = importNormalizer;
        m_importValidator = importValidator;
        m_importProcessor = importProcessor;
    }

    protected IImportReader<TImportRequest, TImportResponse> ImportReader => m_importReader;

    protected IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse> ImportPipelineChannelKeyResolver => m_importPipelineChannelKeyResolver;

    protected IEnumerable<IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>> ImportPipelineChannels => m_importPipelineChannels;

    protected IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> ImportNormalizer => m_importNormalizer;

    protected IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> ImportValidator => m_importValidator;

    protected IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> ImportProcessor => m_importProcessor;

    /// <inheritdoc />
    public async Task<TImportResponse> ImportAsync(TImportRequest importRequest, CancellationToken cancellationToken = default)
    {
        ImportSourceBatch[] importSourceBatches =
            await ReadAsync(importRequest, cancellationToken);

        ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[] importObjectPackageBatches =
            await DeserializeAsync(importRequest, importSourceBatches, cancellationToken);

        await NormalizeAsync(importObjectPackageBatches, cancellationToken);

        await ValidateAsync(importObjectPackageBatches, cancellationToken);

        TImportResponse importResponse =
            await ProcessAsync(importRequest, importObjectPackageBatches, cancellationToken);

        return importResponse;
    }

    protected virtual async Task<ImportSourceBatch[]> ReadAsync(TImportRequest importRequest, CancellationToken cancellationToken = default)
    {
        ImportSourceBatch[] importSourceBatches =
            await ImportReader.ReadAsync(importRequest, cancellationToken);

        return importSourceBatches;
    }

    protected virtual async Task<ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[]> DeserializeAsync(TImportRequest importRequest, ImportSourceBatch[] importSourceBatches, CancellationToken cancellationToken = default)
    {
        ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[] importObjectPackageBatches =
            new ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[importSourceBatches.Length];

        for (int importSourceBatchIndex = 0; importSourceBatchIndex < importSourceBatches.Length; importSourceBatchIndex++)
        {
            ImportSourceBatch importSourceBatch = importSourceBatches[importSourceBatchIndex];

            int size = importSourceBatch.Size;
            ImportSource[] importSources = importSourceBatch.ImportSources;
            ImportSourceContext importSourceContext = importSourceBatch.ImportSourceContext;

            ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage> importObjectPackageBatch = new()
            {
                Size = size,
                ImportSources = importSources,
                ImportSourceContext = importSourceContext,
                ImportIntermediateObjectPackageWrappers = new ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackage>[importSources.Length],
                ImportObjectPackageWrappers = new ImportObjectPackageWrapper<TImportObjectPackage>[importSources.Length],
            };

            importObjectPackageBatch = await DeserializeCoreAsync(importRequest, importObjectPackageBatch, cancellationToken);

            importObjectPackageBatches[importSourceBatchIndex] = importObjectPackageBatch;
        }

        return importObjectPackageBatches;
    }

    protected virtual async Task NormalizeAsync(ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[] importObjectPackageBatches, CancellationToken cancellationToken = default)
    {
        await m_importNormalizer.NormalizeAsync(importObjectPackageBatches, cancellationToken);
    }

    protected virtual async Task ValidateAsync(ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[] importObjectPackageBatches, CancellationToken cancellationToken = default)
    {
        await m_importValidator.ValidateAsync(importObjectPackageBatches, cancellationToken);
    }

    protected virtual async Task<TImportResponse> ProcessAsync(TImportRequest importRequest, ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[] importObjectPackageBatches, CancellationToken cancellationToken = default)
    {
        TImportResponse importResponse =
            await ImportProcessor.ProcessAsync(importRequest, importObjectPackageBatches, cancellationToken);

        return importResponse;
    }

    private async Task<ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>> DeserializeCoreAsync(TImportRequest importRequest, ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage> importObjectPackageBatch, CancellationToken cancellationToken)
    {
        int size = importObjectPackageBatch.Size;
        ImportSource[] importSources = importObjectPackageBatch.ImportSources;
        ImportSourceContext importSourceContext = importObjectPackageBatch.ImportSourceContext;

        ImportPipelineChannelKey[] importPipelineChannelKeys = new ImportPipelineChannelKey[size];
        for (int index = 0; index < size; index++)
        {
            ImportPipelineChannelKey importPipelineChannelKey = m_importPipelineChannelKeyResolver.SupportsAsyncMode
                ? await m_importPipelineChannelKeyResolver.ResolveImportPipelineChannelKeyAsync(importRequest, importSources[index], importSourceContext, cancellationToken)
                : m_importPipelineChannelKeyResolver.ResolveImportPipelineChannelKey(importRequest, importSources[index], importSourceContext);

            importPipelineChannelKeys[index] = importPipelineChannelKey;
        }

        ILookup<ImportPipelineChannelKey, (ImportSource ImportSource, int Index)> importSourceTuplesByImportPipelineChannelKey = importSources
            .Select((importSource, index) => (ImportSource: importSource, Index: index))
            .ToLookup(importSourceTuple => importPipelineChannelKeys[importSourceTuple.Index]);

        foreach (IGrouping<ImportPipelineChannelKey, (ImportSource ImportSource, int Index)> importSourceTupleGrouping in importSourceTuplesByImportPipelineChannelKey)
        {
            ImportPipelineChannelKey importPipelineChannelKey = importSourceTupleGrouping.Key;

            ImportSource[] importSourceGrouping = importSourceTupleGrouping
                .Select(importSourceTuple => importSourceTuple.ImportSource)
                .ToArray();

            int fragmentSize = importSourceGrouping.Length;
            ImportSourceBatch importSourceBatchFragment = new()
            {
                Size = fragmentSize,
                ImportSources = importSourceGrouping,
                ImportSourceContext = importSourceContext,
            };

            ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>? importObjectPackageBatchFragment = null;

            void SetImportObjectPackageBatchFragment(ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>? outImportObjectPackageBatchFragment) =>
                importObjectPackageBatchFragment = outImportObjectPackageBatchFragment;

            TryDeserializeFragmentCoreParameters tryDeserializeFragmentCoreParameters = new()
            {
                ImportPipelineChannelKey = importPipelineChannelKey,
                ImportSourceBatchFragment = importSourceBatchFragment,
                SetImportObjectPackageBatchFragmentCallback = SetImportObjectPackageBatchFragment,
            };

            if (!await TryDeserializeFragmentCoreAsync(tryDeserializeFragmentCoreParameters, cancellationToken) || importObjectPackageBatchFragment is null)
                throw ImportExceptionFactory.CreateNoSuitableImportPipelineChannelException<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(importPipelineChannelKey);

            int fragmentIndex = 0;
            foreach ((ImportSource importSource, int index) in importSourceTupleGrouping)
            {
                importObjectPackageBatch.ImportIntermediateObjectPackageWrappers[index] = importObjectPackageBatchFragment.ImportIntermediateObjectPackageWrappers[fragmentIndex];
                importObjectPackageBatch.ImportObjectPackageWrappers[index] = importObjectPackageBatchFragment.ImportObjectPackageWrappers[fragmentIndex];
                fragmentIndex++;
            }
        }

        return importObjectPackageBatch;
    }

    private async Task<bool> TryDeserializeFragmentCoreAsync(TryDeserializeFragmentCoreParameters parameters, CancellationToken cancellationToken)
    {
        ImportPipelineChannelKey importPipelineChannelKey = parameters.ImportPipelineChannelKey;

#pragma warning disable IDE0018 // Inline variable declaration (If the variable is inlined, the line becomes unnecessarily too long.)
        IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>? importPipelineChannel;
        if (!m_importPipelineChannelsDictionary.TryGetValue(importPipelineChannelKey, out importPipelineChannel))
        {
            parameters.SetImportObjectPackageBatchFragmentCallback(null);
            return false;
        }
#pragma warning restore IDE0018 // Inline variable declaration

        ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage> importObjectPackageBatchFragment =
            await importPipelineChannel.DeserializeAsync(parameters.ImportSourceBatchFragment, cancellationToken);

        parameters.SetImportObjectPackageBatchFragmentCallback(importObjectPackageBatchFragment);
        return true;
    }

    private class TryDeserializeFragmentCoreParameters
    {
        public required ImportPipelineChannelKey ImportPipelineChannelKey { get; init; }

        public required ImportSourceBatch ImportSourceBatchFragment { get; init; }

        public required Action<ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>?> SetImportObjectPackageBatchFragmentCallback { get; init; }
    }
}
