namespace VidiView.Api.Exceptions;

public class E1735_ImageNotFoundException : E1712_NotFoundException
{
    public E1735_ImageNotFoundException(string message)
        : base(1735, message)
    {
    }

    public string? ImageId { get; init; }
}
