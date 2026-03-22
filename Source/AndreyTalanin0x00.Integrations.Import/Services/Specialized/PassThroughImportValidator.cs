using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

namespace AndreyTalanin0x00.Integrations.Import.Services.Specialized;

public class PassThroughImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    /// <inheritdoc />
    public Task ValidateAsync(ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[] importObjectPackageBatches, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
