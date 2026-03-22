namespace AndreyTalanin0x00.Integrations.Import;

public class ImportObjectPackageWrapper<TImportObjectPackage>
    where TImportObjectPackage : class?
{
    public required TImportObjectPackage ImportObjectPackage { get; set; }
}
