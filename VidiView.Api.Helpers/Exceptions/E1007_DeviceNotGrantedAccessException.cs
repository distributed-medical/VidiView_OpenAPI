using VidiView.Api.DataModel;
using VidiView.Api.Serialization;

namespace VidiView.Api.Exceptions;

public class E1007_DeviceNotGrantedAccessException : VidiViewException
{
    public E1007_DeviceNotGrantedAccessException(string message)
        : base(1007, message)
    {
    }

    public string? VerificationCode { get; init; }
}