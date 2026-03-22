namespace AndreyTalanin0x00.Integrations.Import;

public record ImportPipelineChannelKey
{
    public static readonly ImportPipelineChannelKey Empty = new(string.Empty);

    public string Value { get; init; }

    public ImportPipelineChannelKey(string value)
    {
        Value = value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
