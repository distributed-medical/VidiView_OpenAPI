namespace VidiView.Api.Exceptions;

public class E1738_StudyNotFoundException : E1712_NotFoundException
{
    public E1738_StudyNotFoundException(string message)
        : base(1738, message)
    {
    }

    public string? StudyId { get; init; }
}
