using HotelWebApp.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;

/// <summary>
/// Класс контроллера для работы администратора с бронями
/// </summary>
[ApiController]
[Route("api/admin/bookings")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AdminBookingsController : ControllerBase
{
    /// <summary>
    /// Интерфейс сервиса для работы с бронями
    /// </summary>
    private readonly IBookingsService _bookingsService;

    /// <summary>
    /// Конструктор контроллера, устанавливающий класс,
    /// реализующий интрефейс сервиса
    /// </summary>
    /// <param name="bookingsService">Сервис дял работы с бронями</param>
    public AdminBookingsController(IBookingsService bookingsService)
    {
        _bookingsService = bookingsService;
    }
    
    /// <summary>
    /// Конечная точка для получения всех броней
    /// </summary>
    /// <response code="200">Успешное получение всех броней</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<Booking>))]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingsService.GetAll();

        return Ok(bookings);
    }
    
    /// <summary>
    /// Конечная точка для подтверждения бронирования администратором
    /// </summary>
    /// <param name="bookingId">Идентификатор бронирования</param>
    /// <response code="204">Успешное подтверждения брони</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut("{bookingId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ConfirmBooking(int bookingId)
    {
        await _bookingsService.ConfirmBooking(bookingId);
        
        return NoContent();
    }
    
    /// <summary>
    /// Конечная точка для выселения клиента
    /// </summary>
    /// <param name="userId">Идентификатор клиента</param>
    /// <response code="204">Успешное выселение клиента</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut]
    [Route("{userId}/evict")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> EvictClient(int bookingId)
    {
        var booking = await _bookingsService.GetById(bookingId);
        
        await _bookingsService.CancelBooking(booking.ResidentId, bookingId);
        
        return NoContent();
    }
}