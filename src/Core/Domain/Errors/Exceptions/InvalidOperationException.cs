using System.Net;

namespace Core.Domain.Errors.Exceptions;

public class InvalidOperationException : BaseException
{
    public InvalidOperationException(string message, Exception innerException = null) : base(HttpStatusCode.BadRequest, message, innerException)
    {
    }
}