using System;
using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

namespace AndreyTalanin0x00.Integrations.Import.Services;

public class ImportPipeline<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    IImportPipeline<TImportRequest, TImportResponse>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    /// <inheritdoc />
    public Task<TImportResponse> ImportAsync(TImportRequest importRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
