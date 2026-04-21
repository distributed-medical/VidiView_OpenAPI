using VidiView.Api.DataModel;
using VidiView.Api.Serialization;

namespace VidiView.Api.Exceptions;

public class E1003_AccessDeniedException : VidiViewException
{
    public E1003_AccessDeniedException(string message)
        : base(1003, message)
    {
    }

    /// <summary>
    /// Optional department information from which this exception derives
    /// </summary>
    public IdAndName? Department { get; init; }
}
