using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;
public interface IWSActorMessage : IWSMessage
{
    /// <summary>
    /// The actor performing the operation that resulted in this message
    /// </summary>
    public UserAndClient Actor { get; init; }
}
