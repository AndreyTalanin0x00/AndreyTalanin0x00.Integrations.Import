using System;
using System.Collections.ObjectModel;

using AndreyTalanin0x00.Integrations.Import.DependencyInjection;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace AndreyTalanin0x00.Integrations.Import.Options;

internal class ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    public Action<IServiceCollection>? AddImportReaderServiceCollectionVisitor { get; set; }

    public Action<IServiceCollection>? AddImportProcessorServiceCollectionVisitor { get; set; }

    public Action<IServiceCollection>? AddImportNormalizerServiceCollectionVisitor { get; set; }

    public Action<IServiceCollection>? AddImportValidatorServiceCollectionVisitor { get; set; }

    public Action<IServiceCollection>? AddImportPipelineChannelKeyResolverServiceCollectionVisitor { get; set; }

    public Collection<ImportPipelineChannelServiceDescriptor> ImportPipelineChannelServiceDescriptors { get; set; } = [];
}

internal class ImportPipelineOptions<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
}
