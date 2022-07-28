using HotelWebApp.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp;

/// <summary>
/// Обработчик исключений
/// </summary>
public static class ErrorHandler
{
    /// <summary>
    /// Метод для обработки исключений и отправки статусных кодов
    /// </summary>
    /// <param name="context">Контекст Http запроса для отправки ответа</param>
    public static async Task Handle(HttpContext context)
    {
        var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

        var problemDetails = new ProblemDetails();

        switch (exceptionObject.Error)
        {
            case UserExistsException:
                context.Response.Headers.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = 400;
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                problemDetails.Title = "User already exists";
                problemDetails.Status = 400;
                await context.Response.WriteAsJsonAsync(problemDetails);
                break;
            
            case RoomSearchException:
                context.Response.Headers.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = 400;
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                problemDetails.Title = "Room with this id not found";
                problemDetails.Status = 400;
                await context.Response.WriteAsJsonAsync(problemDetails);
                break;
            
            case AuthenticationException:
                context.Response.Headers.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = 401;
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
                problemDetails.Title = "This user does not exist";
                problemDetails.Status = 401;
                await context.Response.WriteAsJsonAsync(problemDetails);
                break;
            
            case AccessException:
                context.Response.Headers.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = 403;
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.2";
                problemDetails.Title = "No access to the requested resource";
                problemDetails.Status = 403;
                await context.Response.WriteAsJsonAsync(problemDetails);
                break;
            
            default:
                context.Response.Headers.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = 500;
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
                problemDetails.Title = "Internal server error";
                problemDetails.Status = 500;
                await context.Response.WriteAsJsonAsync(problemDetails);
                break;
        }
    }
}