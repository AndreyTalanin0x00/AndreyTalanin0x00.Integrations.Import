using System;
using System.Diagnostics.CodeAnalysis;

using AndreyTalanin0x00.Extensions.DependencyInjection;
using AndreyTalanin0x00.Integrations.Import.Options.Builders.Abstractions;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;
using AndreyTalanin0x00.Integrations.Import.Services.Specialized;

using Microsoft.Extensions.DependencyInjection;

namespace AndreyTalanin0x00.Integrations.Import.Options.Builders;

internal class ImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> :
    IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    where TImportObjectPackage : class
{
    private readonly ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> m_importPipelineChannelOptions;

    public ImportPipelineChannelOptionsBuilder(ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importPipelineChannelOptions)
    {
        m_importPipelineChannelOptions = importPipelineChannelOptions;

        if (typeof(TImportIntermediateObjectPackageCurrent) == typeof(TImportObjectPackage))
            UseImportMapper<PassThroughImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>>();

        return;
    }

    /// <inheritdoc />
    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseKey(string importPipelineChannelKey)
    {
        m_importPipelineChannelOptions.ImportPipelineChannelKey = new ImportPipelineChannelKey(importPipelineChannelKey);

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseKey(ImportPipelineChannelKey importPipelineChannelKey)
    {
        m_importPipelineChannelOptions.ImportPipelineChannelKey = importPipelineChannelKey;

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseImportSerializer<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportSerializer>()
        where TImportSerializer : class, IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    {
        m_importPipelineChannelOptions.AddImportSerializerServiceCollectionVisitor = AddImportSerializer;

        static void AddImportSerializer(IServiceCollection services) =>
            services.AddTransient<IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>, TImportSerializer>();

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseImportSerializer<TImportSerializer>(ServiceImplementationFactory<TImportSerializer> importSerializerImplementationFactory)
        where TImportSerializer : class, IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    {
        m_importPipelineChannelOptions.AddImportSerializerServiceCollectionVisitor = AddImportSerializer;

        void AddImportSerializer(IServiceCollection services) =>
            services.AddTransient<IImportSerializer<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>, TImportSerializer>(serviceProvider => importSerializerImplementationFactory(serviceProvider));

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseImportMapper<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportMapper>()
        where TImportMapper : class, IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    {
        m_importPipelineChannelOptions.AddImportMapperServiceCollectionVisitor = AddImportMapper;

        static void AddImportMapper(IServiceCollection services) =>
            services.AddTransient<IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>, TImportMapper>();

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> UseImportMapper<TImportMapper>(ServiceImplementationFactory<TImportMapper> importMapperImplementationFactory)
        where TImportMapper : class, IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    {
        m_importPipelineChannelOptions.AddImportMapperServiceCollectionVisitor = AddImportMapper;

        void AddImportMapper(IServiceCollection services) =>
            services.AddTransient<IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>, TImportMapper>(serviceProvider => importMapperImplementationFactory(serviceProvider));

        return this;
    }

    /// <inheritdoc />
    public ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> Build()
    {
        Assert();

        return m_importPipelineChannelOptions;
    }

    protected virtual void Assert()
    {
        if (m_importPipelineChannelOptions.AddImportSerializerServiceCollectionVisitor is null)
            throw new InvalidOperationException("The import pipeline channel's configuration is invalid: no import serializer is specified.");
        if (m_importPipelineChannelOptions.AddImportMapperServiceCollectionVisitor is null)
            throw new InvalidOperationException("The import pipeline channel's configuration is invalid: no import mapper is specified.");

        return;
    }
}

internal class ImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> :
    ImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>,
    IImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    where TImportObjectPackage : class
{
    private readonly ImportPipelineChannelOptions<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> m_importPipelineChannelOptions;

    public ImportPipelineChannelOptionsBuilder(ImportPipelineChannelOptions<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importPipelineChannelOptions)
        : base(importPipelineChannelOptions)
    {
        m_importPipelineChannelOptions = importPipelineChannelOptions;
    }

    /// <inheritdoc />
    public new ImportPipelineChannelOptions<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> Build()
    {
        Assert();

        return m_importPipelineChannelOptions;
    }
}
