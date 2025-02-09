using System.Net;

namespace Core.Domain.Errors.Exceptions;

public class BusinessRuleException : BaseException
{
    public BusinessRuleException(string message, Exception innerException = null) : base(HttpStatusCode.UnprocessableEntity, message, innerException)
    {
    }
}