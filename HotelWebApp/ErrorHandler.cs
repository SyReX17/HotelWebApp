using HotelWebApp.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace HotelWebApp;

public static class ErrorHandler
{
    public static async Task Handle(HttpContext context)
    {
        var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

        switch (exceptionObject.Error)
        {
            case AuthenticationException:
                context.Response.StatusCode = 401;
                break;
                
            case RequestException:
                context.Response.StatusCode = 400;
                break;
                
            default:
                context.Response.StatusCode = 500;
                break;
        }
    }
}