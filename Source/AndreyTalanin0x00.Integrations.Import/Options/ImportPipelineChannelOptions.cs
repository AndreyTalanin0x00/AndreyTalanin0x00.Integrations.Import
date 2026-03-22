using System;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace AndreyTalanin0x00.Integrations.Import.Options;

internal class ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    where TImportObjectPackage : class
{
    public ImportPipelineChannelKey? ImportPipelineChannelKey { get; set; }

    public Action<IServiceCollection>? AddImportSerializerServiceCollectionVisitor { get; set; }

    public Action<IServiceCollection>? AddImportMapperServiceCollectionVisitor { get; set; }
}

internal class ImportPipelineChannelOptions<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> :
    ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    where TImportObjectPackage : class
{
}
