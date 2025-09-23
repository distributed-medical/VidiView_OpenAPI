namespace VidiView.Api.Exceptions;

public class E1760_DepartmentNotFoundException : E1712_NotFoundException
{
    public E1760_DepartmentNotFoundException(string message)
        : base(message)
    {
    }

    public string? DepartmentId { get; init; }
}
