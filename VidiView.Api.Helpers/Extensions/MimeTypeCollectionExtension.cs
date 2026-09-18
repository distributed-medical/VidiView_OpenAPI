using VidiView.Api.DataModel;

namespace VidiView.Api.Extensions;

public static class MimeTypeCollectionExtension
{
    /// <summary>
    /// Get Image/jpeg MimeType from the collection
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static MimeType ImageJpeg(this MimeTypeCollection collection)
    {
        return collection["image/jpeg"];
    }

    /// <summary>
    /// Get Image/png MimeType from the collection
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static MimeType ImagePng(this MimeTypeCollection collection)
    {
        return collection["image/png"];
    }

    /// <summary>
    /// Get Video/mp4 MimeType from the collection
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static MimeType VideoMp4(this MimeTypeCollection collection)
    {
        return collection["video/mp4"];
    }

    /// <summary>
    /// Parse the DICOM subtype from the MimeType if it is a DICOM type.
    /// </summary>
    /// <param name="mimeType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string? GetDicomSubType(this MimeType mimeType)
    {
        ArgumentNullException.ThrowIfNull(mimeType, nameof(mimeType));
        if (!mimeType.IsDicomType)
        {
            throw new ArgumentException($"MimeType '{mimeType.Type}' is not a DICOM type.", nameof(mimeType));
        }

        string contentType = mimeType.Type;

        int ix = contentType.IndexOf("pixel-data=");
        if (ix > 0)
        {
            return contentType[(ix + 11)..];
        }

        ix = contentType.IndexOf("waveform-data=");
        if (ix > 0)
        {
            return contentType[(ix + 14)..];
        }

        return null;
    }
}
