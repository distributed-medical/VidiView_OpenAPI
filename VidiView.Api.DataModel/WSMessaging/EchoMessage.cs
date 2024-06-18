namespace VidiView.Api.WSMessaging;

public class EchoMessage : WSMessage
{
    public string EchoText { get; set; } = string.Empty;
}