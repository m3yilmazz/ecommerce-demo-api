namespace Core.Domain.Errors.Models;

public class ErrorResponse
{
    public string Message { get; set; }
    public string ExceptionMessage { get; set; }

    public ErrorResponse() { }

    public ErrorResponse(string message, string exceptionMessage = null)
    {
        Message = message;
        ExceptionMessage = exceptionMessage;
    }
}