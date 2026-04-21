namespace VidiView.Api.Exceptions;

public class E2120_SnapshotsNotSupportedException : VidiViewException
{
    public E2120_SnapshotsNotSupportedException(string message)
        : base(2120, message)
    {
    }
}
