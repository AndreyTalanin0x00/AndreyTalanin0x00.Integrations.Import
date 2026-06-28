using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AndreyTalanin0x00.Extensions.DependencyInjection;
using AndreyTalanin0x00.Integrations.Import.DependencyInjection;
using AndreyTalanin0x00.Integrations.Import.Options.Builders.Abstractions;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;
using AndreyTalanin0x00.Integrations.Import.Services.Specialized;

using Microsoft.Extensions.DependencyInjection;

// Disable the IDE0001 (Simplify name) notification to preserve explicit types.
#pragma warning disable IDE0001

namespace AndreyTalanin0x00.Integrations.Import.Options.Builders;

internal class ImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    private readonly ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> m_importPipelineOptions;

    public ImportPipelineOptionsBuilder(ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importPipelineOptions)
    {
        m_importPipelineOptions = importPipelineOptions;

        UseImportNormalizer<PassThroughImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>>();
        UseImportValidator<PassThroughImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>>();
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportReader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportReader>()
        where TImportReader : class, IImportReader<TImportRequest, TImportResponse>
    {
        m_importPipelineOptions.AddImportReaderServiceCollectionVisitor = AddImportReader;

        static void AddImportReader(IServiceCollection services) =>
            services.AddTransient<IImportReader<TImportRequest, TImportResponse>, TImportReader>();

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportReader<TImportReader>(ServiceImplementationFactory<TImportReader> importReaderImplementationFactory)
        where TImportReader : class, IImportReader<TImportRequest, TImportResponse>
    {
        m_importPipelineOptions.AddImportReaderServiceCollectionVisitor = AddImportReader;

        void AddImportReader(IServiceCollection services) =>
            services.AddTransient<IImportReader<TImportRequest, TImportResponse>, TImportReader>(serviceProvider => importReaderImplementationFactory(serviceProvider));

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportNormalizer<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportNormalizer>()
        where TImportNormalizer : class, IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    {
        m_importPipelineOptions.AddImportNormalizerServiceCollectionVisitor = AddImportNormalizer;

        static void AddImportNormalizer(IServiceCollection services) =>
            services.AddTransient<IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>, TImportNormalizer>();

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportNormalizer<TImportNormalizer>(ServiceImplementationFactory<TImportNormalizer> importNormalizerImplementationFactory)
        where TImportNormalizer : class, IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    {
        m_importPipelineOptions.AddImportNormalizerServiceCollectionVisitor = AddImportNormalizer;

        void AddImportNormalizer(IServiceCollection services) =>
            services.AddTransient<IImportNormalizer<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>, TImportNormalizer>(serviceProvider => importNormalizerImplementationFactory(serviceProvider));

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportValidator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportValidator>()
        where TImportValidator : class, IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    {
        m_importPipelineOptions.AddImportValidatorServiceCollectionVisitor = AddImportValidator;

        static void AddImportValidator(IServiceCollection services) =>
            services.AddTransient<IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>, TImportValidator>();

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportValidator<TImportValidator>(ServiceImplementationFactory<TImportValidator> importValidatorImplementationFactory)
        where TImportValidator : class, IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    {
        m_importPipelineOptions.AddImportValidatorServiceCollectionVisitor = AddImportValidator;

        void AddImportValidator(IServiceCollection services) =>
            services.AddTransient<IImportValidator<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>, TImportValidator>(serviceProvider => importValidatorImplementationFactory(serviceProvider));

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportProcessor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportProcessor>()
        where TImportProcessor : class, IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    {
        m_importPipelineOptions.AddImportProcessorServiceCollectionVisitor = AddImportProcessor;

        static void AddImportProcessor(IServiceCollection services) =>
            services.AddTransient<IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>, TImportProcessor>();

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportProcessor<TImportProcessor>(ServiceImplementationFactory<TImportProcessor> importProcessorImplementationFactory)
        where TImportProcessor : class, IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    {
        m_importPipelineOptions.AddImportProcessorServiceCollectionVisitor = AddImportProcessor;

        void AddImportProcessor(IServiceCollection services) =>
            services.AddTransient<IImportProcessor<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>, TImportProcessor>(serviceProvider => importProcessorImplementationFactory(serviceProvider));

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportPipelineChannelKeyResolver<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportPipelineChannelKeyResolver>()
        where TImportPipelineChannelKeyResolver : class, IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse>
    {
        m_importPipelineOptions.AddImportPipelineChannelKeyResolverServiceCollectionVisitor = AddImportPipelineChannelKeyResolver;

        static void AddImportPipelineChannelKeyResolver(IServiceCollection services) =>
            services.AddTransient<IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse>, TImportPipelineChannelKeyResolver>();

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> UseImportPipelineChannelKeyResolver<TImportPipelineChannelKeyResolver>(ServiceImplementationFactory<TImportPipelineChannelKeyResolver> importPipelineChannelKeyResolverImplementationFactory)
        where TImportPipelineChannelKeyResolver : class, IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse>
    {
        m_importPipelineOptions.AddImportPipelineChannelKeyResolverServiceCollectionVisitor = AddImportPipelineChannelKeyResolver;

        void AddImportPipelineChannelKeyResolver(IServiceCollection services) =>
            services.AddTransient<IImportPipelineChannelKeyResolver<TImportRequest, TImportResponse>, TImportPipelineChannelKeyResolver>(serviceProvider => importPipelineChannelKeyResolverImplementationFactory(serviceProvider));

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> AddImportPipelineChannel<TImportIntermediateObjectPackageCurrent>(Action<IImportPipelineChannelOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>> configureAction)
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    {
        AddCustomImportPipelineChannelCore<ImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>, TImportIntermediateObjectPackageCurrent>(configureAction: configureAction);

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> AddCustomImportPipelineChannel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(Action<IImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>> configureAction)
        where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    {
        AddCustomImportPipelineChannelCore<TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(null, configureAction);

        return this;
    }

    /// <inheritdoc />
    public IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> AddCustomImportPipelineChannel<TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(KeyedServiceImplementationFactory<TImportPipelineChannel, ImportPipelineChannelKey> importPipelineChannelImplementationFactory, Action<IImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>> configureAction)
        where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    {
        AddCustomImportPipelineChannelCore<TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(importPipelineChannelImplementationFactory, configureAction);

        return this;
    }

    /// <inheritdoc />
    public ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> Build()
    {
        Assert();

        return m_importPipelineOptions;
    }

    protected virtual void Assert()
    {
        if (m_importPipelineOptions.AddImportReaderServiceCollectionVisitor is null)
            throw new InvalidOperationException("The import pipeline's configuration is invalid: no import reader is specified.");
        if (m_importPipelineOptions.AddImportNormalizerServiceCollectionVisitor is null)
            throw new InvalidOperationException("The import pipeline's configuration is invalid: no import normalizer is specified.");
        if (m_importPipelineOptions.AddImportValidatorServiceCollectionVisitor is null)
            throw new InvalidOperationException("The import pipeline's configuration is invalid: no import validator is specified.");
        if (m_importPipelineOptions.AddImportProcessorServiceCollectionVisitor is null)
            throw new InvalidOperationException("The import pipeline's configuration is invalid: no import processor is specified.");
        if (m_importPipelineOptions.AddImportPipelineChannelKeyResolverServiceCollectionVisitor is null)
            throw new InvalidOperationException("The import pipeline's configuration is invalid: no import pipeline channel key resolver is specified.");

        int importPipelineChannelServiceDescriptorsCount = m_importPipelineOptions.ImportPipelineChannelServiceDescriptors.Count;
        if (importPipelineChannelServiceDescriptorsCount == 0)
            throw new UnreachableException("The import pipeline's configuration is invalid: no import pipeline channel is specified.");

        int importPipelineChannelServiceDescriptorsKeylessCount = m_importPipelineOptions.ImportPipelineChannelServiceDescriptors
            .Where(importPipelineChannelServiceDescriptor => importPipelineChannelServiceDescriptor.ImportPipelineChannelKey is null)
            .Count();
        if (importPipelineChannelServiceDescriptorsKeylessCount > 1)
            throw new UnreachableException("The import pipeline's configuration is invalid: the import pipeline has multiple keyless import pipeline channels. Only a single one is allowed.");

        int importPipelineChannelServiceDescriptorsUniqueCount = m_importPipelineOptions.ImportPipelineChannelServiceDescriptors
            .Select(importPipelineChannelServiceDescriptor => importPipelineChannelServiceDescriptor.ImportPipelineChannelKey?.Value ?? string.Empty)
            .Distinct()
            .Count();
        if (importPipelineChannelServiceDescriptorsUniqueCount != importPipelineChannelServiceDescriptorsCount)
            throw new UnreachableException("The import pipeline's configuration is invalid: the import pipeline has non-unique import pipeline channels keys.");

        return;
    }

    private ImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> AddCustomImportPipelineChannelCore<TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(KeyedServiceImplementationFactory<TImportPipelineChannel, ImportPipelineChannelKey>? importPipelineChannelImplementationFactory = null, Action<IImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>>? configureAction = null)
        where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    {
        ImportPipelineChannelOptions<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importPipelineChannelOptions = new();
        ImportPipelineChannelOptionsBuilder<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importPipelineChannelOptionsBuilder = new(importPipelineChannelOptions);

        if (configureAction is not null)
            configureAction(importPipelineChannelOptionsBuilder);

        importPipelineChannelOptions = importPipelineChannelOptionsBuilder.Build();

        ImportPipelineChannelKey importPipelineChannelKey = importPipelineChannelOptions.ImportPipelineChannelKey
            ?? ImportPipelineChannelKey.Empty;

        ImportPipelineChannelServiceDescriptor importPipelineChannelServiceDescriptor = new()
        {
            ImportPipelineChannelKey = importPipelineChannelKey,
            AddImportPipelineChannelServiceCollectionVisitor = AddImportPipelineChannelCore,
        };

        m_importPipelineOptions.ImportPipelineChannelServiceDescriptors.Add(importPipelineChannelServiceDescriptor);

        void AddImportPipelineChannelCore(IServiceCollection services)
        {
            ConfigureImportPipelineChannelServices<TImportIntermediateObjectPackageCurrent>(services, importPipelineChannelOptions);
            if (typeof(TImportPipelineChannel) != typeof(ImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>))
                ConfigureCustomImportPipelineChannelServices<TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(services, importPipelineChannelOptions);

            if (importPipelineChannelImplementationFactory is not null)
            {
                TImportPipelineChannel GetImportPipelineChannelByKey(IServiceProvider serviceProvider, object? serviceKey) =>
                    importPipelineChannelImplementationFactory(serviceProvider, (ImportPipelineChannelKey)serviceKey!);

                services.AddKeyedTransient<IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>, TImportPipelineChannel>(importPipelineChannelKey, (serviceProvider, serviceKey) => GetImportPipelineChannelByKey(serviceProvider, serviceKey));
            }
            else
            {
                services.AddKeyedTransient<IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>, TImportPipelineChannel>(importPipelineChannelKey);
            }

            IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> GetImportPipelineChannel(IServiceProvider serviceProvider) =>
                serviceProvider.GetRequiredKeyedService<IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>>(importPipelineChannelKey);

            services.AddTransient<IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>>(GetImportPipelineChannel);
        }

        return this;
    }

    private static void ConfigureImportPipelineChannelServices<TImportIntermediateObjectPackageCurrent>(IServiceCollection services, ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importPipelineChannelOptions)
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    {
        const string importPipelineChannelOptionsClassName =
            nameof(ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>);

        const string addImportSerializerServiceCollectionVisitorPropertyName =
            nameof(ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>.AddImportSerializerServiceCollectionVisitor);
        const string addImportMapperServiceCollectionVisitorPropertyName =
            nameof(ImportPipelineChannelOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>.AddImportMapperServiceCollectionVisitor);

        const string buildMethodName =
            nameof(IImportPipelineChannelOptionsBuilderInternal<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>.Build);

        Action<IServiceCollection> addImportSerializerServiceCollectionVisitor = importPipelineChannelOptions.AddImportSerializerServiceCollectionVisitor
            ?? throw new UnreachableException($"An {importPipelineChannelOptionsClassName} instance has its {addImportSerializerServiceCollectionVisitorPropertyName} property set to null after the {buildMethodName} method has been called");
        Action<IServiceCollection> addImportMapperServiceCollectionVisitor = importPipelineChannelOptions.AddImportMapperServiceCollectionVisitor
            ?? throw new UnreachableException($"An {importPipelineChannelOptionsClassName} instance has its {addImportMapperServiceCollectionVisitorPropertyName} property set to null after the {buildMethodName} method has been called");

        addImportSerializerServiceCollectionVisitor(services);
        addImportMapperServiceCollectionVisitor(services);
    }

    private static void ConfigureCustomImportPipelineChannelServices<TImportPipelineChannel, TImportIntermediateObjectPackageCurrent>(IServiceCollection services, ImportPipelineChannelOptions<TImportPipelineChannel, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importPipelineChannelOptions)
        where TImportPipelineChannel : class, IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
        where TImportIntermediateObjectPackageCurrent : class, TImportIntermediateObjectPackage
    {
    }
}

internal class ImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> :
    ImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>,
    IImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    private readonly ImportPipelineOptions<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> m_importPipelineOptions;

    public ImportPipelineOptionsBuilder(ImportPipelineOptions<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importPipelineOptions)
        : base(importPipelineOptions)
    {
        m_importPipelineOptions = importPipelineOptions;
    }

    /// <inheritdoc />
    public new ImportPipelineOptions<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> Build()
    {
        Assert();

        return m_importPipelineOptions;
    }
}
