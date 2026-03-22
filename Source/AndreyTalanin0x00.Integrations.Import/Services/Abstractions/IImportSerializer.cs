using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;

namespace AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

public interface IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackageCurrent : class
    where TImportObjectPackage : class
{
    public bool AcceptsBlobsOfMimeType(string blobMimeType);

    public Task<ImportIntermediateObjectPackageBatch<TImportIntermediateObjectPackageCurrent, TImportObjectPackage>> DeserializeAsync(ImportIntermediateObjectPackageBatch<TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importIntermediateObjectPackageBatch, CancellationToken cancellationToken = default);
}
