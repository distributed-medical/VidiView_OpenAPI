namespace VidiView.Api.WSMessaging;

[ExcludeFromCodeCoverage]
public record EchoMessage : IWSMessage
{
    public EchoMessage()
    {
        // Maybe it seems odd to use the ToString() here instead
        // of FullName, but the ToString will not return assembly qualified name
        // for generic type parameters
        MessageType = this.GetType().ToString();
        MessageId = Guid.NewGuid().ToString("N");
    }

    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string Text { get; set; } = string.Empty;
}