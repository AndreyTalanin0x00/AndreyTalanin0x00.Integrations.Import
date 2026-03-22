namespace AndreyTalanin0x00.Integrations.Import;

public class ImportSourceBatch
{
    public required int Size { get; set; }

    public required ImportSource[] ImportSources { get; set; }

    public required ImportSourceContext ImportSourceContext { get; set; }
}
