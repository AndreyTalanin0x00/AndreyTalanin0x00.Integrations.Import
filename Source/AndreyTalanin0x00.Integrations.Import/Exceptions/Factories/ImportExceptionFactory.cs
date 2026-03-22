using System;

using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

namespace AndreyTalanin0x00.Integrations.Import.Exceptions.Factories;

public static class ImportExceptionFactory
{
    public static InvalidOperationException CreateNoSuitableImportPipelineChannelException<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>(ImportPipelineChannelKey importPipelineChannelKey)
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackage : class
        where TImportObjectPackage : class
    {
        const string importPipelineChannelInterfaceName =
            nameof(IImportPipelineChannel<TImportRequest, TImportResponse, TImportIntermediateObjectPackage, TImportObjectPackage>);

        const string exceptionMessageFormat = ""
            + "Could not find a suitable " + importPipelineChannelInterfaceName + " for this import. "
            + "The import pipeline channel key that could not be handled: {0}. "
            + "Generic parameters: {1}, {2}, {3}, {4}. "
            + "Check import services (dependency injection container configuration).";

        string exceptionMessage = string.Format(exceptionMessageFormat, importPipelineChannelKey.Value, typeof(TImportRequest), typeof(TImportResponse), typeof(TImportIntermediateObjectPackage), typeof(TImportObjectPackage));

        InvalidOperationException exception = new(exceptionMessage);

        return exception;
    }

    public static InvalidOperationException CreateUnsupportedImportSourceBlobMimeTypeException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>(ImportSource importSource)
    {
        const string exceptionMessageFormat = ""
            + "Could not deserialize an import source. "
            + "The import source blob's MIME type is not supported: {0}. "
            + "Generic parameters: {1}, {2}, {3}, {4}. "
            + "Check import services (API consumer import mapper implementation).";

        string exceptionBlobReference = $"{importSource.BlobMetadata.FileName} ({importSource.BlobReference})";
        string exceptionMessage = string.Format(exceptionMessageFormat, exceptionBlobReference, typeof(TImportRequest), typeof(TImportResponse), typeof(TImportIntermediateObjectPackageCurrent), typeof(TImportObjectPackage));

        InvalidOperationException exception = new(exceptionMessage);

        return exception;
    }

    public static NullReferenceException CreateImportSourceDeserializedAsNullException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>(ImportSource importSource)
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackageCurrent : class
        where TImportObjectPackage : class
    {
        const string exceptionMessageFormat = ""
            + "Could not deserialize an import source. "
            + "The import source was deserialized as null: {0}. "
            + "Generic parameters: {1}, {2}, {3}, {4}. "
            + "Check import services (API consumer import mapper implementation).";

        string exceptionBlobReference = $"{importSource.BlobMetadata.FileName} ({importSource.BlobReference})";
        string exceptionMessage = string.Format(exceptionMessageFormat, exceptionBlobReference, typeof(TImportRequest), typeof(TImportResponse), typeof(TImportIntermediateObjectPackageCurrent), typeof(TImportObjectPackage));

        NullReferenceException exception = new(exceptionMessage);

        return exception;
    }

    public static NullReferenceException CreateImportSourceMappedAsNullException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>(ImportSource importSource)
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackageCurrent : class
        where TImportObjectPackage : class
    {
        const string exceptionMessageFormat = ""
            + "Could not map an import source blob. "
            + "The import source was mapped as null: {0}. "
            + "Generic parameters: {1}, {2}, {3}, {4}. "
            + "Check import services (API consumer import mapper implementation).";

        string exceptionBlobReference = $"{importSource.BlobMetadata.FileName} ({importSource.BlobReference})";
        string exceptionMessage = string.Format(exceptionMessageFormat, exceptionBlobReference, typeof(TImportRequest), typeof(TImportResponse), typeof(TImportIntermediateObjectPackageCurrent), typeof(TImportObjectPackage));

        NullReferenceException exception = new(exceptionMessage);

        return exception;
    }

    public static ArgumentException CreateImportIntermediateObjectPackageWrapperCountMismatchException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>()
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackageCurrent : class
        where TImportObjectPackage : class
    {
        const string exceptionMessageFormat = ""
            + "Could not process an import intermediate object package batch. "
            + "The object was not created correctly: arrays' lengths do not match. "
            + "Generic parameters: {0}, {1}, {2}, {3}. "
            + "Check import services (API consumer services - import pipeline channel).";

        string exceptionMessage = string.Format(exceptionMessageFormat, typeof(TImportRequest), typeof(TImportResponse), typeof(TImportIntermediateObjectPackageCurrent), typeof(TImportObjectPackage));

        ArgumentException exception = new(exceptionMessage);

        return exception;
    }

    public static ArgumentException CreateImportObjectPackageWrapperCountMismatchException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>()
        where TImportRequest : ImportRequest
        where TImportResponse : ImportResponse
        where TImportIntermediateObjectPackageCurrent : class
        where TImportObjectPackage : class
    {
        const string exceptionMessageFormat = ""
            + "Could not process an import object package batch. "
            + "The object was not created correctly: arrays' lengths do not match. "
            + "Generic parameters: {0}, {1}, {2}, {3}. "
            + "Check import services (API consumer services - import pipeline channel).";

        string exceptionMessage = string.Format(exceptionMessageFormat, typeof(TImportRequest), typeof(TImportResponse), typeof(TImportIntermediateObjectPackageCurrent), typeof(TImportObjectPackage));

        ArgumentException exception = new(exceptionMessage);

        return exception;
    }
}
