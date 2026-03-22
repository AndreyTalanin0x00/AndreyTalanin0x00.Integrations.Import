using System;
using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

namespace AndreyTalanin0x00.Integrations.Import.Services;

public class ImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> :
    IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    where TImportObjectPackage : class
{
    /// <inheritdoc />
    public ImportPipelineChannelKey Key => throw new NotImplementedException();

    /// <inheritdoc />
    public Task<ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>> DeserializeAsync(ImportSourceBatch importSourceBatch, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
