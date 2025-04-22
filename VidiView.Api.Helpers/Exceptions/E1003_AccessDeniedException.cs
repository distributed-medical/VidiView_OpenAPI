using VidiView.Api.DataModel;

namespace VidiView.Api.Exceptions;

public class E1003_AccessDeniedException : VidiViewException
{
    public E1003_AccessDeniedException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Optional department information from which this exception derives
    /// </summary>
    public IdAndName? Department { get; init; }
}
