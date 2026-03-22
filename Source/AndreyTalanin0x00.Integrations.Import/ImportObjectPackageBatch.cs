namespace AndreyTalanin0x00.Integrations.Import;

public class ImportObjectPackageBatch<TImportIntermediateObjectPackage, TImportObjectPackage>
    where TImportIntermediateObjectPackage : class
    where TImportObjectPackage : class
{
    public required int Size { get; set; }

    public required ImportSource[] ImportSources { get; set; }

    public required ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackage>[] ImportIntermediateObjectPackageWrappers { get; set; }

    public required ImportObjectPackageWrapper<TImportObjectPackage>[] ImportObjectPackageWrappers { get; set; }

    public required ImportSourceContext ImportSourceContext { get; set; }
}
