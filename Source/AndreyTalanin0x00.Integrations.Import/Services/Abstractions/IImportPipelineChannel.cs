using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;

namespace AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

public interface IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    public ImportPipelineChannelKey Key { get; }

    public Task<ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>> DeserializeAsync(ImportSourceBatch importSourceBatch, CancellationToken cancellationToken = default);
}
