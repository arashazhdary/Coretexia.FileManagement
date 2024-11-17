namespace Coretexia.FileManagement.Abstraction.Models.Common;

public class FileManagementError
{

    public FileManagementError(string error, Exception? exception = null)
    {
        Error = error;
        Exception = exception;
    }

    public string Error { get; set; }
    public Exception? Exception { get; set; }
}