namespace VidiView.Api.DataModel;
public enum ImageStatus
{
    /// <summary>
    /// Image metadata announced, 
    /// </summary>
    Announced = 0,

    /// <summary>
    /// Image file received and verified
    /// </summary>
    Verified = 20,

    /// <summary>
    /// File has been erased on the server
    /// </summary>
    FileErased = 80,
}
