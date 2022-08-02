using HotelWebApp.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.MIddlewares;

public class ErrorHandlerMiddleware
{
    private RequestDelegate next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (RequestException e)
        {
            var problemDetails = new ProblemDetails();

            context.Response.Headers.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = e.StatusCode;
            problemDetails.Title = e.Message;
            problemDetails.Status = e.StatusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception e)
        {
            var problemDetails = new ProblemDetails();
            
            context.Response.Headers.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 500;
            problemDetails.Title = "Внутренняя ошибка сервера";
            problemDetails.Status = 500;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}