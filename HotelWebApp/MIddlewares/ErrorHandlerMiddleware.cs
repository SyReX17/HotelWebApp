﻿using HotelWebApp.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.MIddlewares;

/// <summary>
/// Класс midllware для обработки ошибок
/// </summary>
public class ErrorHandlerMiddleware
{
    /// <summary>
    /// Фабрика для создания логгера
    /// </summary>
    private readonly ILoggerFactory _loggerFactory;
    /// <summary>
    /// Логгер для вывода внутренних ошибок сервера
    /// </summary>
    private ILogger<Program> _logger;
    /// <summary>
    /// Ссыка на следущий middleware
    /// </summary>
    private RequestDelegate next;

    /// <summary>
    /// Конструктор обработчика принимает ссылку на следущий middleware,
    /// инициализирует логгер при помощи фабрики
    /// </summary>
    /// <param name="next">Ссылка на следуший middleware</param>
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        next = next;
        _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = _loggerFactory.CreateLogger<Program>();
    }

    /// <summary>
    /// Метод для отлова ошибок при помощи try/catch, принимает
    /// контекст http запроса из предыдущего middleware
    /// </summary>
    /// <param name="context">Контекст http запроса</param>
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
            _logger.LogError(e.Message);
            var problemDetails = new ProblemDetails();
            
            context.Response.Headers.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 500;
            problemDetails.Title = "Внутренняя ошибка сервера";
            problemDetails.Status = 500;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}