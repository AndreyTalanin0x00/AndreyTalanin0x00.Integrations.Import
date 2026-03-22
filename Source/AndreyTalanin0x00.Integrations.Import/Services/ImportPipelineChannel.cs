using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Exceptions.Factories;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

// Disable the IDE0032 (Use auto property) notification to preserve easily recognizable 'injected services' field pattern.
#pragma warning disable IDE0032

namespace AndreyTalanin0x00.Integrations.Import.Services;

public class ImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> :
    IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    where TImportObjectPackage : class
{
    private readonly ImportPipelineChannelKey m_importPipelineChannelKey;
    private readonly IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> m_importSerializer;
    private readonly IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> m_importMapper;

    public ImportPipelineChannel(
        [ServiceKey] ImportPipelineChannelKey importPipelineChannelKey,
        IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importSerializer,
        IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importMapper)
    {
        m_importPipelineChannelKey = importPipelineChannelKey;
        m_importSerializer = importSerializer;
        m_importMapper = importMapper;
    }

    /// <inheritdoc />
    public ImportPipelineChannelKey Key => m_importPipelineChannelKey;

    /// <inheritdoc />
    public async Task<ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>> DeserializeAsync(ImportSourceBatch importSourceBatch, CancellationToken cancellationToken = default)
    {
        int size = importSourceBatch.Size;

        ImportSource[] importSources = importSourceBatch.ImportSources;

        ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage> importObjectPackageBatch;
        ImportIntermediateObjectPackageBatch<TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importIntermediateObjectPackageBatch;

        importIntermediateObjectPackageBatch = new()
        {
            Size = size,
            ImportSources = importSourceBatch.ImportSources,
            ImportSourceContext = importSourceBatch.ImportSourceContext,
            ImportIntermediateObjectPackageWrappers = new ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackageCurrent>[size],
            ImportObjectPackageWrappers = new ImportObjectPackageWrapper<TImportObjectPackage>[size],
        };

        importIntermediateObjectPackageBatch = await m_importSerializer.DeserializeAsync(importIntermediateObjectPackageBatch, cancellationToken);

        importIntermediateObjectPackageBatch = m_importMapper.Map(importIntermediateObjectPackageBatch);

        importObjectPackageBatch = new()
        {
            Size = size,
            ImportSources = importSourceBatch.ImportSources,
            ImportSourceContext = importSourceBatch.ImportSourceContext,
            ImportIntermediateObjectPackageWrappers = new ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackage>[size],
            ImportObjectPackageWrappers = new ImportObjectPackageWrapper<TImportObjectPackage>[size],
        };

        ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackageCurrent>[] importIntermediateObjectPackageWrappers =
            importIntermediateObjectPackageBatch.ImportIntermediateObjectPackageWrappers;
        ImportObjectPackageWrapper<TImportObjectPackage>[] importObjectPackageWrappers =
            importIntermediateObjectPackageBatch.ImportObjectPackageWrappers;

        for (int index = 0; index < size; index++)
        {
            ImportSource importSource = importSources[index];

            ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackageCurrent> importIntermediateObjectPackageWrapper =
                importIntermediateObjectPackageWrappers[index];
            ImportObjectPackageWrapper<TImportObjectPackage> importObjectPackageWrapper =
                importObjectPackageWrappers[index];

            if (importIntermediateObjectPackageWrapper.ImportIntermediateObjectPackage is null)
                throw ImportExceptionFactory.CreateImportSourceDeserializedAsNullException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>(importSource);
            if (importObjectPackageWrapper.ImportObjectPackage is null)
                throw ImportExceptionFactory.CreateImportSourceMappedAsNullException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>(importSource);

            importObjectPackageBatch.ImportIntermediateObjectPackageWrappers[index] = new ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackage>()
            {
                ImportIntermediateObjectPackage = importIntermediateObjectPackageWrapper.ImportIntermediateObjectPackage,
            };
            importObjectPackageBatch.ImportObjectPackageWrappers[index] = new ImportObjectPackageWrapper<TImportObjectPackage>()
            {
                ImportObjectPackage = importObjectPackageWrapper.ImportObjectPackage,
            };
        }

        return importObjectPackageBatch;
    }
}
