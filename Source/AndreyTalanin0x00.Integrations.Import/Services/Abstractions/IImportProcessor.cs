using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;

namespace AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

public interface IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    public Task<TImportResponse> ProcessAsync(TImportRequest importRequest, ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>[] importObjectPackageBatches, CancellationToken cancellationToken = default);
}
