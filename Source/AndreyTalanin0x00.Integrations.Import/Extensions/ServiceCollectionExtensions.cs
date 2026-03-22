using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using AndreyTalanin0x00.Extensions.DependencyInjection;
using AndreyTalanin0x00.Integrations.Import.DependencyInjection;
using AndreyTalanin0x00.Integrations.Import.Options;
using AndreyTalanin0x00.Integrations.Import.Options.Builders;
using AndreyTalanin0x00.Integrations.Import.Options.Builders.Abstractions;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

using ObjectImportIntermediateObjectPackage = System.Object;

// Disable the IDE0001 (Simplify name) notification to preserve explicit types.
#pragma warning disable IDE0001

namespace AndreyTalanin0x00.Integrations.Import.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImportPipeline<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(this IServiceCollection services, Action<IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>> configureAction)
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackage : class
        where TImportObjectPackage : class
    {
        services.AddImportPipelineCore<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(configureAction);

        return services;
    }

    public static IServiceCollection AddImportPipeline<TImportRequest, TImportResponse, TImportObjectPackage>(this IServiceCollection services, Action<IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, ObjectImportIntermediateObjectPackage, TImportObjectPackage>> configureAction)
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportObjectPackage : class
    {
        services.AddImportPipelineCore<TImportRequest, TImportResponse, ObjectImportIntermediateObjectPackage, TImportObjectPackage>(configureAction);

        return services;
    }

    public static IServiceCollection AddCustomImportPipeline<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(this IServiceCollection services, Action<IImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>> configureAction)
        where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackage : class
        where TImportObjectPackage : class
    {
        services.AddCustomImportPipelineCore<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(null, configureAction);

        return services;
    }

    public static IServiceCollection AddCustomImportPipeline<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(this IServiceCollection services, ServiceImplementationFactory<TImportPipeline> importPipelineImplementationFactory, Action<IImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>> configureAction)
        where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackage : class
        where TImportObjectPackage : class
    {
        services.AddCustomImportPipelineCore<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(importPipelineImplementationFactory, configureAction);

        return services;
    }

    public static IServiceCollection AddCustomImportPipeline<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImportPipeline, TImportRequest, TImportResponse, TImportObjectPackage>(this IServiceCollection services, Action<IImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, ObjectImportIntermediateObjectPackage, TImportObjectPackage>> configureAction)
        where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportObjectPackage : class
    {
        services.AddCustomImportPipelineCore<TImportPipeline, TImportRequest, TImportResponse, ObjectImportIntermediateObjectPackage, TImportObjectPackage>(null, configureAction);

        return services;
    }

    public static IServiceCollection AddCustomImportPipeline<TImportPipeline, TImportRequest, TImportResponse, TImportObjectPackage>(this IServiceCollection services, ServiceImplementationFactory<TImportPipeline> importPipelineImplementationFactory, Action<IImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, ObjectImportIntermediateObjectPackage, TImportObjectPackage>> configureAction)
        where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportObjectPackage : class
    {
        services.AddCustomImportPipelineCore<TImportPipeline, TImportRequest, TImportResponse, ObjectImportIntermediateObjectPackage, TImportObjectPackage>(importPipelineImplementationFactory, configureAction);

        return services;
    }

    private static IServiceCollection AddImportPipelineCore<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(this IServiceCollection services, Action<IImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>>? configureAction)
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackage : class
        where TImportObjectPackage : class
    {
        ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importPipelineOptions = new();
        ImportPipelineOptionsBuilder<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importPipelineOptionsBuilder = new(importPipelineOptions);

        if (configureAction is not null)
            configureAction(importPipelineOptionsBuilder);

        importPipelineOptions = importPipelineOptionsBuilder.Build();

        ConfigureImportPipelineServices(services, importPipelineOptions);

        services.AddTransient<IImportPipeline<TImportRequest, TImportResponse>, ImportPipeline<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>>();

        return services;
    }

    private static IServiceCollection AddCustomImportPipelineCore<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(this IServiceCollection services, ServiceImplementationFactory<TImportPipeline>? importPipelineImplementationFactory, Action<IImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>>? configureAction)
        where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackage : class
        where TImportObjectPackage : class
    {
        ImportPipelineOptions<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importPipelineOptions = new();
        ImportPipelineOptionsBuilder<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importPipelineOptionsBuilder = new(importPipelineOptions);

        if (configureAction is not null)
            configureAction(importPipelineOptionsBuilder);

        importPipelineOptions = importPipelineOptionsBuilder.Build();

        ConfigureImportPipelineServices(services, importPipelineOptions);
        ConfigureCustomImportPipelineServices(services, importPipelineOptions);

        if (importPipelineImplementationFactory is not null)
            services.AddTransient<IImportPipeline<TImportRequest, TImportResponse>, TImportPipeline>(serviceProvider => importPipelineImplementationFactory(serviceProvider));
        else
            services.AddTransient<IImportPipeline<TImportRequest, TImportResponse>, TImportPipeline>();

        return services;
    }

    private static void ConfigureImportPipelineServices<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(IServiceCollection services, ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importPipelineOptions)
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackage : class
        where TImportObjectPackage : class
    {
        const string importPipelineOptionsClassName =
            nameof(ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>);

        const string addImportReaderServiceCollectionVisitorPropertyName =
            nameof(ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>.AddImportReaderServiceCollectionVisitor);
        const string addImportProcessorServiceCollectionVisitorPropertyName =
            nameof(ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>.AddImportProcessorServiceCollectionVisitor);
        const string addImportValidatorServiceCollectionVisitorPropertyName =
            nameof(ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>.AddImportValidatorServiceCollectionVisitor);
        const string addImportPipelineChannelKeyResolverServiceCollectionVisitorPropertyName =
            nameof(ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>.AddImportPipelineChannelKeyResolverServiceCollectionVisitor);
        const string importPipelineChannelServiceDescriptorsPropertyName =
            nameof(ImportPipelineOptions<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>.ImportPipelineChannelServiceDescriptors);

        const string buildMethodName =
            nameof(IImportPipelineOptionsBuilderInternal<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>.Build);

        Action<IServiceCollection> addImportReaderServiceCollectionVisitor = importPipelineOptions.AddImportReaderServiceCollectionVisitor
            ?? throw new UnreachableException($"An {importPipelineOptionsClassName} instance has its {addImportReaderServiceCollectionVisitorPropertyName} property set to null after the {buildMethodName} method has been called.");
        Action<IServiceCollection> addImportProcessorServiceCollectionVisitor = importPipelineOptions.AddImportProcessorServiceCollectionVisitor
            ?? throw new UnreachableException($"An {importPipelineOptionsClassName} instance has its {addImportProcessorServiceCollectionVisitorPropertyName} property set to null after the {buildMethodName} method has been called.");
        Action<IServiceCollection> addImportValidatorServiceCollectionVisitor = importPipelineOptions.AddImportValidatorServiceCollectionVisitor
            ?? throw new UnreachableException($"An {importPipelineOptionsClassName} instance has its {addImportValidatorServiceCollectionVisitorPropertyName} property set to null after the {buildMethodName} method has been called.");
        Action<IServiceCollection> addImportPipelineChannelKeyResolverServiceCollectionVisitor = importPipelineOptions.AddImportPipelineChannelKeyResolverServiceCollectionVisitor
            ?? throw new UnreachableException($"An {importPipelineOptionsClassName} instance has its {addImportPipelineChannelKeyResolverServiceCollectionVisitorPropertyName} property set to null after the {buildMethodName} method has been called.");

        if (importPipelineOptions.ImportPipelineChannelServiceDescriptors.Count == 0)
            throw new UnreachableException($"An {importPipelineOptionsClassName} instance has its {importPipelineChannelServiceDescriptorsPropertyName} property empty after the {buildMethodName} method has been called.");

        addImportReaderServiceCollectionVisitor(services);
        addImportProcessorServiceCollectionVisitor(services);
        addImportValidatorServiceCollectionVisitor(services);
        addImportPipelineChannelKeyResolverServiceCollectionVisitor(services);

        foreach (ImportPipelineChannelServiceDescriptor importPipelineChannelServiceDescriptor in importPipelineOptions.ImportPipelineChannelServiceDescriptors)
        {
            Action<IServiceCollection> addImportPipelineChannelServiceCollectionVisitor =
                importPipelineChannelServiceDescriptor.AddImportPipelineChannelServiceCollectionVisitor;

            addImportPipelineChannelServiceCollectionVisitor(services);
        }
    }

    private static void ConfigureCustomImportPipelineServices<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(IServiceCollection services, ImportPipelineOptions<TImportPipeline, TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage> importPipelineOptions)
        where TImportPipeline : class, IImportPipeline<TImportRequest, TImportResponse>
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackage : class
        where TImportObjectPackage : class
    {
    }
}
