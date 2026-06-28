using System;
using System.Diagnostics.CodeAnalysis;

using AndreyTalanin0x00.Extensions.DependencyInjection;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

namespace AndreyTalanin0x00.Integrations.Import.Options.Builders.Abstractions;

public interface IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportReader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportReader>()
        where TImportReader : class, IImportReader<TImportRequest, TImportResponse>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportReader<TImportReader>(ServiceImplementationFactory<TImportReader> importReaderImplementationFactory)
        where TImportReader : class, IImportReader<TImportRequest, TImportResponse>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportNormalizer<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportNormalizer>()
        where TImportNormalizer : class, IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportNormalizer<TImportNormalizer>(ServiceImplementationFactory<TImportNormalizer> importNormalizerImplementationFactory)
        where TImportNormalizer : class, IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportValidator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportValidator>()
        where TImportValidator : class, IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportValidator<TImportValidator>(ServiceImplementationFactory<TImportValidator> importValidatorImplementationFactory)
        where TImportValidator : class, IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportProcessor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportProcessor>()
        where TImportProcessor : class, IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportProcessor<TImportProcessor>(ServiceImplementationFactory<TImportProcessor> importProcessorImplementationFactory)
        where TImportProcessor : class, IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportPipelineChannelKeyResolver<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportPipelineChannelKeyResolver>()
        where TImportPipelineChannelKeyResolver : class, IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportPipelineChannelKeyResolver<TImportPipelineChannelKeyResolver>(ServiceImplementationFactory<TImportPipelineChannelKeyResolver> importPipelineChannelKeyResolverImplementationFactory)
        where TImportPipelineChannelKeyResolver : class, IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse>;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> AddImportPipelineChannel<TImportIntermediateObjectPackageCurrent>(Action<IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>> configureAction)
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> AddCustomImportPipelineChannel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(Action<IImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>> configureAction)
        where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage;

    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> AddCustomImportPipelineChannel<TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(KeyedServiceImplementationFactory<TImportPipelineChannel, ImportPipelineChannelKey> importPipelineChannelImplementationFactory, Action<IImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>> configureAction)
        where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage;
}

internal interface IImportPipelineOptionsBuilderInternal<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    public ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> Build();
}

public interface IImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
}

internal interface IImportPipelineOptionsBuilderInternal<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    IImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    public ImportPipelineOptions<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> Build();
}
