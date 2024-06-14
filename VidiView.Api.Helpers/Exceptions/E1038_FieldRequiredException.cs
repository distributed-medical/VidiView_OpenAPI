using VidiView.Api.DataModel;
using VidiView.Api.Serialization;

namespace VidiView.Api.Exceptions;

public class E1038_FieldRequiredException : VidiViewException
{
    public E1038_FieldRequiredException(string message)
        : base(message)
    {
    }

    public string? FieldLevel => Problem.GetPropertyValue<string>(nameof(FieldLevel), VidiViewJson.DefaultOptions);

}
