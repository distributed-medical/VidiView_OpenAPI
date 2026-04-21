using VidiView.Api.DataModel;
using VidiView.Api.Serialization;

namespace VidiView.Api.Exceptions;

public class E1038_FieldRequiredException : VidiViewException
{
    public E1038_FieldRequiredException(string message)
        : base(1038, message)
    {
    }

    public string? FieldLevel { get; init; }
    public string? FieldName { get; init; }
}
