using System;
using System.Diagnostics.CodeAnalysis;

using AndreyTalanin0x00.Extensions.DependencyInjection;
using AndreyTalanin0x00.Integrations.Import.Options.Builders.Abstractions;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

namespace AndreyTalanin0x00.Integrations.Import.Options.Builders;

internal class ImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> :
    IImportPipelineChannelOptionsBuilderInternal<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    where TImportObjectPackage : class
{
    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseKey(string importPipelineChannelKey)
    {
        throw new NotImplementedException();
    }

    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseKey(ImportPipelineChannelKey importPipelineChannelKey)
    {
        throw new NotImplementedException();
    }

    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseImportSerializer<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportSerializer>()
        where TImportSerializer : class, IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    {
        throw new NotImplementedException();
    }

    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseImportSerializer<TImportSerializer>(ServiceImplementationFactory<TImportSerializer> importSerializerImplementationFactory)
        where TImportSerializer : class, IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    {
        throw new NotImplementedException();
    }

    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseImportMapper<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportMapper>()
        where TImportMapper : class, IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    {
        throw new NotImplementedException();
    }

    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseImportMapper<TImportMapper>(ServiceImplementationFactory<TImportMapper> importMapperImplementationFactory)
        where TImportMapper : class, IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    {
        throw new NotImplementedException();
    }

    public ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> Build()
    {
        throw new NotImplementedException();
    }
}

internal class ImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> :
    ImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>,
    IImportPipelineChannelOptionsBuilderInternal<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    where TImportObjectPackage : class
{
    public new ImportPipelineChannelOptions<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> Build()
    {
        throw new NotImplementedException();
    }
}
