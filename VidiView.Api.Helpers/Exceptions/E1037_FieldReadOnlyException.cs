using VidiView.Api.DataModel;
using VidiView.Api.Serialization;

namespace VidiView.Api.Exceptions;

public class E1037_FieldReadOnlyException : VidiViewException
{
    public E1037_FieldReadOnlyException(string message)
        : base(message)
    {
    }

    public string? FieldLevel { get; init; }
    public string? FieldName { get; init; }
}
