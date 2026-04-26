# AndreyTalanin0x00.Integrations.Import

A set of abstractions to add generic import pipeline capabilities to a .NET application.

Core services include:

- `IImportPipeline` - Defines the contract for an import pipeline that processes import requests and produces import responses.
- `IImportPipelineChannel` - Defines the contract for a channel within an import pipeline that can process intermediate object packages. Different channels handle different file formats (e.g., XML, CSV).
- `IImportPipelineChannelKeyResolver` - Resolves the appropriate channel key based on the import request.
- `IImportReader` - Reads import blobs containing serialized files to import. Each import blob can be translated to multiple object packages in case of arrays.
- `IImportValidator` - Validates an application-level DTO model package before it is imported.
- `IImportProcessor` - Processes an import request with a collection of application-level DTO model packages and passes final data to the application.

An export pipeline channel consists of multiple services:

- `IImportSerializer` - Deserializes a blob of a specific file format (e.g., XML, CSV) into a serializable model package.
- `IImportMapper` - Maps a serializable model package to an application-level DTO model package.

The import pipeline can be configured in a Fluent API style, allowing for easy setup and customization of import channels and their associated services:

```csharp
// This extension method is taken from the 'JapaneseLanguageTools' project, where it is used to set up an import pipeline for importing the application dictionary in both JSON and XML formats.
// See the original source code here: https://github.com/AndreyTalanin0x00/JapaneseLanguageTools/blob/development-preview/Source/JapaneseLanguageTools.Core.Import/Extensions/ServiceCollectionExtensions.cs
public static IServiceCollection AddApplicationDictionaryImportPipeline(this IServiceCollection services)
{
    services.AddImportPipeline<ApplicationDictionaryImportRequest, ApplicationDictionaryImportResponse, Object, ApplicationDictionaryObjectPackageIntegrationModel>(importPipelineOptionsBuilder =>
    {
        importPipelineOptionsBuilder
            .UseImportReader<ApplicationDictionaryImportReader>()
            .AddImportPipelineChannel<ApplicationDictionaryObjectPackageJsonModel>(importPipelineChannelOptionsBuilder =>
            {
                importPipelineChannelOptionsBuilder
                    .UseKey(ApplicationDictionaryImportPipelineChannelKeys.ApplicationDictionaryImportPipelineChannelKeyJson)
                    .UseImportSerializer<ApplicationDictionaryJsonImportSerializer>()
                    .UseImportMapper<ApplicationDictionaryJsonImportMapper>();
            })
            .AddImportPipelineChannel<ApplicationDictionaryObjectPackageXmlModel>(importPipelineChannelOptionsBuilder =>
            {
                importPipelineChannelOptionsBuilder
                    .UseKey(ApplicationDictionaryImportPipelineChannelKeys.ApplicationDictionaryImportPipelineChannelKeyXml)
                    .UseImportSerializer<ApplicationDictionaryXmlImportSerializer>()
                    .UseImportMapper<ApplicationDictionaryXmlImportMapper>();
            })
            .UseImportPipelineChannelKeyResolver<ApplicationDictionaryImportPipelineChannelKeyResolver>()
            .UseImportValidator<ApplicationDictionaryImportValidator>()
            .UseImportProcessor<ApplicationDictionaryImportProcessor>();
    });

    return services;
}
```
