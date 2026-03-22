using System;

using AndreyTalanin0x00.Integrations.Import.Exceptions.Factories;
using AndreyTalanin0x00.Integrations.Import.Requests;
using AndreyTalanin0x00.Integrations.Import.Responses;
using AndreyTalanin0x00.Integrations.Import.Services.Abstractions;

namespace AndreyTalanin0x00.Integrations.Import.Services.Specialized;

public class PassThroughImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage> :
    IImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>
    where TImportRequest : ImportRequest
    where TImportResponse : ImportResponse
    where TImportIntermediateObjectPackageCurrent : class
    where TImportObjectPackage : class
{
    /// <inheritdoc />
    public ImportIntermediateObjectPackageBatch<TImportIntermediateObjectPackageCurrent, TImportObjectPackage> Map(ImportIntermediateObjectPackageBatch<TImportIntermediateObjectPackageCurrent, TImportObjectPackage> importIntermediateObjectPackageBatch)
    {
        if (typeof(TImportIntermediateObjectPackageCurrent) == typeof(TImportObjectPackage))
        {
            int size = importIntermediateObjectPackageBatch.Size;

            ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackageCurrent>[] importIntermediateObjectPackageWrappers =
                importIntermediateObjectPackageBatch.ImportIntermediateObjectPackageWrappers;
            ImportObjectPackageWrapper<TImportObjectPackage>[] importObjectPackageWrappers =
                importIntermediateObjectPackageBatch.ImportObjectPackageWrappers;

            if (size != importIntermediateObjectPackageWrappers.Length)
                throw ImportExceptionFactory.CreateImportIntermediateObjectPackageWrapperCountMismatchException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>();
            if (size != importObjectPackageWrappers.Length)
                throw ImportExceptionFactory.CreateImportObjectPackageWrapperCountMismatchException<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>();

            for (int index = 0; index < size; index++)
            {
                const string passThroughImportMapperClassName =
                    nameof(PassThroughImportMapper<TImportRequest, TImportResponse, TImportIntermediateObjectPackageCurrent, TImportObjectPackage>);

                ImportObjectPackageWrapper<TImportObjectPackage> importObjectPackageWrapper =
                    ((object)importIntermediateObjectPackageBatch.ImportIntermediateObjectPackageWrappers[index]) as ImportObjectPackageWrapper<TImportObjectPackage>
                    ?? throw new InvalidCastException($"Unable to use the {passThroughImportMapperClassName} import mapper implementation, import object package types are different.");

                importIntermediateObjectPackageBatch.ImportObjectPackageWrappers[index] = importObjectPackageWrapper;
            }
        }

        return importIntermediateObjectPackageBatch;
    }
}
