using System.Net;

namespace Core.Domain.Errors.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message, Exception innerException = null) : base(HttpStatusCode.BadRequest, message, innerException)
    {
    }
}