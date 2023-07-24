using Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[AllowAnonymous]
public class ErrorsController : ControllerBase
{
    [Route("error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (statusCode, message) = exception switch
        {
            IServiceException serviceException => (serviceException.StatusCode, serviceException.ErrorMessage),
            ValidationExceptions ex => (ex.Code, ex.ToString()),
            _ => (500, "Unknown error occured.")
        };

        return Problem(title: "Error", detail: message, statusCode: statusCode);
    }
}
