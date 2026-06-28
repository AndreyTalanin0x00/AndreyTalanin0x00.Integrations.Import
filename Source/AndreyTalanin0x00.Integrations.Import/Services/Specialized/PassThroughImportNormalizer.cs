using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

namespace AndreyTalanin0x00.Integrations.Import.Services.Specialized;

public class PassThroughImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    /// <inheritdoc />
    public Task NormalizeAsync(ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[] importObjectPackageBatches, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
