using HotelWebApp.Interfaces.Services;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;

/// <summary>
/// Класс контроллера для работы администратора с счетами на оплату
/// </summary>
[ApiController]
[Route("api/admin/invoices")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AdminInvoicesController : ControllerBase
{
    /// <summary>
    /// Реализация сервиса для работы с счетами на оплату
    /// </summary>
    private readonly IInvoicesService _invoicesService;

    /// <summary>
    /// Конструктор контроллера, устанавливающий класс,
    /// реализующий интерфейс репозитория
    /// </summary>
    /// <param name="invoicesService">Сервис для работы с счетами на оплату</param>
    public AdminInvoicesController(IInvoicesService invoicesService)
    {
        _invoicesService = invoicesService;
    }
    
    /// <summary>
    /// Конечная точка для получения счетов на оплату
    /// </summary>
    /// <response code="200">Успешное получение счетов на оплату</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(List<Invoice>))]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetInvoices()
    {
        var invoices = await _invoicesService.GetInvoices();
        
        return Ok(invoices);
    }
    
    /// <summary>
    /// Конечная точка для подтверждения оплаты
    /// </summary>
    /// <param name="invoiceId">Идентификатор счета на оплату</param>
    /// <response code="204">Успешное подтверждение оплаты</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("{invoiceId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ConfirmInvoice(int invoiceId)
    {
        await _invoicesService.ConfirmInvoice(invoiceId);
        return NoContent();
    }
}