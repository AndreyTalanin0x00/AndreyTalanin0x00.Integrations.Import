using AndreyTalanin0x00.Integrations.Blobs;

namespace AndreyTalanin0x00.Integrations.Import;

public class ImportResource
{
    public required BlobReference BlobReference { get; set; }

    public required BlobMetadata BlobMetadata { get; set; }

    public required string VirtualFilePath { get; set; }
}
