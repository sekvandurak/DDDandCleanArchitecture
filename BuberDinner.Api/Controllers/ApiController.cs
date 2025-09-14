using BuberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{   
    protected IActionResult Problem(List <Error> errors)
    {
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;
        
        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.Validation => 400,
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.Failure => 500,
            ErrorType.Unauthorized => 401,
            _ => 500
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }

}