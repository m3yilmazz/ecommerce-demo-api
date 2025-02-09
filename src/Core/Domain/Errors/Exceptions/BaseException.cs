using System.Net;

namespace Core.Domain.Errors.Exceptions;

public abstract class BaseException : Exception
{
    public HttpStatusCode StatusCode { get; }

    protected BaseException()
    {

    }

    protected BaseException(HttpStatusCode statusCode, string message, Exception innerException = null) : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}