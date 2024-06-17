namespace VidiView.Api.WSMessaging;
public class AuthenticateMessage : WSMessage
{
    public AuthenticateMessage(string apiKey, string authorization)
    {
        ApiKey = apiKey;
        Authorization = authorization;
    }

    public string ApiKey { get; init; }
    public string Authorization { get; init; }
}
