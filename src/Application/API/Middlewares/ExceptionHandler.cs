using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Errors.Models;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace Application.API.Middlewares;

/// <summary>
/// Middleware that handles exceptions and returns a standardized error response.
/// </summary>
public class ExceptionHandler
{
    /// <summary>
    /// Handles the exception and writes the appropriate response.
    /// </summary>
    /// <param name="httpContext">The HTTP context of the current request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task Handle(HttpContext httpContext)
    {
        var unitOfWork = httpContext.RequestServices.GetService<IUnitOfWork>();

        if (unitOfWork?.HasTransaction == true) await unitOfWork.RollbackTransactionAsync();

        var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature!.Error;

        var response = httpContext.Response;
        response.ContentType = "application/json";

        if (exception is BaseException)
            response.StatusCode = (int)((exception as BaseException)?.StatusCode ?? System.Net.HttpStatusCode.InternalServerError);

        var result = JsonConvert.SerializeObject(new ErrorResponse(exception.Message, exception.InnerException?.Message));

        await response.WriteAsync(result);
    }
}