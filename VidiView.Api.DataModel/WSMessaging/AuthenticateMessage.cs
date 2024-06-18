namespace VidiView.Api.WSMessaging;

public class AuthenticateMessage : WSMessage
{
    public AuthenticateMessage()
    {
    }

    public AuthenticateMessage(string apiKey, string authorization)
    {
        ApiKey = apiKey;
        Authorization = authorization;
    }

    public string ApiKey { get; set; }
    public string Authorization { get; set; }
}