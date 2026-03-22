using AndreyTalanin0x00.Integrations.Blobs;

namespace AndreyTalanin0x00.Integrations.Import.Requests;

public class ImportRequest
{
    public required BlobReference[] BlobReferences { get; set; } = [];
}
