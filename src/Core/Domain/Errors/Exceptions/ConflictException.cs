using System.Net;

namespace Core.Domain.Errors.Exceptions;

public class ConflictException : BaseException
{
    public ConflictException(string message, Exception innerException = null) : base(HttpStatusCode.Conflict, message, innerException)
    {
    }
}