namespace VidiView.Api.Exceptions;

public class E1039_DeserializeException : VidiViewException
{
    public E1039_DeserializeException(Type expectedType, Exception? innerException)
        : base($"Failed to deserialize <{expectedType.Name}>", innerException)
    {
        ErrorCode = 1039;
        ExpectedType = expectedType;
    }

    public E1039_DeserializeException(string message, Type expectedType, Exception? innerException)
        : base(message, innerException)
    {
        ErrorCode = 1039;
        ExpectedType = expectedType;
    }

    public E1039_DeserializeException(string message)
    : base(message)
    {
        ErrorCode = 1039;
    }

    /// <summary>
    /// The expected data type
    /// </summary>
    public Type? ExpectedType { get; }

    /// <summary>
    /// The raw response
    /// </summary>
    public string? RawResponse { get; init; }
}