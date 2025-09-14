using System.Net;

namespace BuberDinner.Api.Middleware;

public class ErrorHandlingMiddleware
{   
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
           await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        var result = System.Text.Json.JsonSerializer.Serialize(new { error = "Bir error gerceklesti" });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return  context.Response.WriteAsync(result);
    }
}