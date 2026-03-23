namespace AndreyTalanin0x00.Integrations.Import.Responses;

public class ImportResponseMessage
{
    public required ImportResponseMessageType Type { get; set; }

    public required string Text { get; set; }
}
