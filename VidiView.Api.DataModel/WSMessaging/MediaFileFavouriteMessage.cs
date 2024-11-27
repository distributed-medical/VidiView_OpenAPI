using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to clients that are reviewing a specific study
/// </summary>
public class MediaFileFavouriteMessage : IWSActorMessage
{
    public MediaFileFavouriteMessage()
    {
        // Maybe it seems odd to use the ToString() here instead
        // of FullName, but the ToString will not return assembly qualified name
        // for generic type parameters
        MessageType = this.GetType().ToString();
        MessageId = Guid.NewGuid().ToString("N");
    }

    public string MessageType { get; init; }
    public string MessageId { get; init; }

    /// <summary>
    /// The user performing the operation
    /// </summary>
    public Actor Actor { get; init; }

    /// <summary>
    /// The study this message is intended for
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The image which has been favourite marked
    /// </summary>
    public Guid ImageId { get; init; }

    /// <summary>
    /// True if favourite, false otherwise
    /// </summary>
    public bool IsFavourite { get; init; }
}
