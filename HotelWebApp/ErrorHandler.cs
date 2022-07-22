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
            
            case AccessException:
                context.Response.StatusCode = 403;
                break;
                
            default:
                context.Response.StatusCode = 500;
                break;
        }
    }
}