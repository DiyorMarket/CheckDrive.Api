using CheckDrive.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CheckDrive.Api.Middlewares;

public class ErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private static async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, message) = GetErrorDetails(exception);
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Detail = message,
        };
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static (int statusCode, string title, string message) GetErrorDetails(Exception ex)
        => ex switch
        {
            EntityNotFoundException => ((int)HttpStatusCode.NotFound, "Not Found", ex.Message),
            InvalidLoginAttemptException => ((int)HttpStatusCode.Unauthorized, "Forbidden", ex.Message),
            RegistrationFailedException => ((int)HttpStatusCode.Unauthorized, "Forbidden", ex.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error", "Unexpected error occurred. Please, try again later."),
        };
}
