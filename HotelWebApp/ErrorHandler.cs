using HotelWebApp.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

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

        switch (exceptionObject.Error)
        {
            case AuthenticationException:
                context.Response.StatusCode = 401;
                break;
            
            case AccessException:
                context.Response.StatusCode = 403;
                break;
                
            default:
                context.Response.StatusCode = 500;
                break;
        }
    }
}