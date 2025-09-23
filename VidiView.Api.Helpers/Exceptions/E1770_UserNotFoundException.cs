namespace VidiView.Api.Exceptions;

public class E1770_UserNotFoundException : E1712_NotFoundException
{
    public E1770_UserNotFoundException(string message)
        : base(message)
    {
    }

    public string? UserId { get; init; }
}
