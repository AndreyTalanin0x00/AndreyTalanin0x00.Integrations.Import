using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;

namespace AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

public interface IImportPipeline<TImportRequest, TImportResponse>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
{
    public Task<TImportResponse> ImportAsync(TImportRequest importRequest, CancellationToken cancellationToken = default);
}
