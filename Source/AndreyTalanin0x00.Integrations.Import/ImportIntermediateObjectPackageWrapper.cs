namespace AndreyTalanin0x00.Integrations.Import;

public class ImportIntermediateObjectPackageWrapper<TImportIntermediateObjectPackage>
    where TImportIntermediateObjectPackage : class?
{
    public required TImportIntermediateObjectPackage ImportIntermediateObjectPackage { get; set; }
}
