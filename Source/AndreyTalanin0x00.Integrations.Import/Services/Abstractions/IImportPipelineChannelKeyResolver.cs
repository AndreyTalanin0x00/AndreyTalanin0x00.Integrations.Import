using System.Threading;
using System.Threading.Tasks;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;

namespace AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

public interface IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
{
    public bool SupportsAsyncMode { get; }

    public Task<ImportPipelineChannelKey> ResolveImportPipelineChannelKeyAsync(TImportRequest importRequest, ImportSource importSource, ImportSourceContext importSourceContext, CancellationToken cancellationToken = default);

    public ImportPipelineChannelKey ResolveImportPipelineChannelKey(TImportRequest importRequest, ImportSource importSource, ImportSourceContext importSourceContext);
}
