namespace Coretexia.FileManagement.FileSystem.Extensions.Exceptions;

public static class ExceptionExtensions
{
    public static string GetInnerMessage(this Exception exception) => getInnerMessage(exception);

    private static string getInnerMessage(Exception exception)
    {
        if (exception.InnerException is null)
            return exception.Message;

        return getInnerMessage(exception);
    }
}
