using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers;

/// <summary>
/// Helps to implementing Mediator pattern.
/// </summary>
public class BaseController : ControllerBase
{
    /// <summary>
    /// Mediator instance to be able to accessing MediatR features.
    /// </summary>
    protected IMediator Mediator { get; }

    /// <summary>
    /// Constructor that makes dependency injection to initialize Mediator instance.
    /// </summary>
    /// <param name="mediator">The mediator used for sending commands and queries.</param>
    public BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }
}