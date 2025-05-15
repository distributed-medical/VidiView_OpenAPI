using VidiView.Api.DataModel;

namespace VidiView.Api.WSMessaging;

/// <summary>
/// This is sent to all clients having a specific study open, when the conference pointer is moved
/// </summary>
[ExcludeFromCodeCoverage]
public record ConferencePointerMessage : IWSReply
{
    public ConferencePointerMessage()
    {
        // Maybe it seems odd to use the ToString() here instead
        // of FullName, but the ToString will not return assembly qualified name
        // for generic type parameters
        MessageType = this.GetType().ToString();
        MessageId = Guid.NewGuid().ToString("N");
    }

    public string MessageType { get; init; }
    public string MessageId { get; init; }
    public string InReplyTo { get; init; }

    /// <summary>
    /// The user performing the operation
    /// </summary>
    public Actor Actor { get; init; }

    /// <summary>
    /// Study this conference relates to
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The source id that the pointer relates to
    /// </summary>
    public Guid? SourceId { get; init; }

    /// <summary>
    /// The media file id that the pointer relates to
    /// </summary>
    public Guid? ImageId { get; init; }

    /// <summary>
    /// Pointer position relative top-left corner
    /// </summary>
    /// <remarks>0 <= X < PixelWidth, 0 <= Y < PixelHeight</remarks>
    public PointInt Position { get; init;}

    /// <summary>
    /// This is the pointer index of the current pointer. 
    /// </summary>
    /// <remarks>
    /// To start sending pointer coordinates, this must be incremented by the client
    /// </remarks>
    public uint PointerIndex { get; init; }

    /// <summary>
    /// Returns true if the current user is owning the pointer
    /// </summary>
    public bool IsOwner { get; init; }

    /// <summary>
    /// Hide pointer
    /// </summary>
    public bool? Hide { get; init; }
}

