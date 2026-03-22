using AndreyTalanin0x00.Integrations.Import.Enumerations;

namespace AndreyTalanin0x00.Integrations.Import.Responses;

public class ImportResponse
{
    public required ImportStatus Status { get; set; }

    public required ImportResponseMessage[] Messages { get; set; }
}
