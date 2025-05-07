namespace VidiView.Api.Helpers;

public static class ExceptionExtension
{
    /// <summary>
    /// Walk the exception and check if any exception is of the desired type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="exception"></param>
    /// <param name="typedException"></param>
    /// <returns></returns>
    public static bool TryGetInnerException<T>(this Exception? exception, out T typedException)
        where T : Exception 
    {
        Exception? inner = exception;
        while (inner != null)
        {
            if (inner is T exc)
            {
                typedException = exc;
                return true;
            }
            inner = inner.InnerException;
        }

        typedException = null!;
        return false;
    }
}
