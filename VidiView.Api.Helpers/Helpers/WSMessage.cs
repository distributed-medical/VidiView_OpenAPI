using VidiView.Api.WSMessaging;

namespace VidiView.Api.Helpers;

public static class WSMessage
{
    /// <summary>
    /// Create an instance of the specific message type
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <returns></returns>
    public static TMessage Factory<TMessage>()
        where TMessage : IWSMessage, new()
    {
        var instance = new TMessage
        {
            // Maybe it seems odd to use the ToString() here instead
            // of FullName, but the ToString will not return assembly qualified name
            // for generic type parameters
            MessageType = typeof(TMessage).ToString(),
            MessageId = Guid.NewGuid().ToString("N")
        };

        return instance;
    }

    /// <summary>
    /// Create an instance of a message reply
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="inReplyTo"></param>
    /// <returns></returns>
    public static TMessage Factory<TMessage>(IWSMessage inReplyTo)
        where TMessage : IWSReply, new()
    {
        var instance = new TMessage
        {
            MessageType = typeof(TMessage).FullName ?? typeof(TMessage).Name,
            MessageId = Guid.NewGuid().ToString("N"),
            InReplyTo = inReplyTo.MessageId,
        };

        return instance;
    }
}