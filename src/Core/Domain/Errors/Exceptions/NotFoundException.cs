using System.Net;

namespace Core.Domain.Errors.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message, Exception innerException = null) : base(HttpStatusCode.NotFound, message, innerException)
    {
    }
}