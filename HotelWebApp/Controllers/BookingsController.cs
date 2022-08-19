using HotelWebApp.Exceptions;
using HotelWebApp.Interfaces.Services;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;

/// <summary>
/// Контроллер для работы с бронированием комнат
/// </summary>
[ApiController]
[Route("api/bookings")]
[Produces("application/json")]
public class BookingsController : ControllerBase
{
    /// <summary>
    /// Интерфейс сервиса для работы с бронями
    /// </summary>
    private readonly IBookingsService _bookingsService;

    /// <summary>
    /// Интерфейсс сервиса для работы с пользователями
    /// </summary>
    private readonly IUsersService _usersService;

    /// <summary>
    /// Конструктор контроллера, устанавливает классы
    /// реализующие интерфейсы сервисов
    /// </summary>
    /// <param name="bookingsService">Сервис для работы с бронями</param>
    /// <param name="usersService">Сервис для работы с пользователями</param>
    public BookingsController(IBookingsService bookingsService, IUsersService usersService)
    {
        _bookingsService = bookingsService;
        _usersService = usersService;
    }
    
    /// <summary>
    /// Конечная точка для добавления новой брони
    /// </summary>
    /// <param name="bookingData">Данные о новой брони полученные из тела запроса</param>
    /// <exception cref="DatesValidationException">Даты введены неверно</exception>
    /// <response code="204">Успешное добавление брони</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(201, Type = typeof(Booking))]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> AddBooking([FromBody] UserBookingData bookingData)
    {
        var booking = await _bookingsService.Add(bookingData, HttpContext.User.Identity.Name);

        return CreatedAtAction(nameof(AddBooking), booking);
    }

    /// <summary>
    /// Конечная точка для продления бронирования
    /// </summary>
    /// <param name="extendData">Данные для прдления бронирования</param>
    /// <response code="204">Успешное продление бронирования</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut]
    [Authorize]
    [Route("{bookingId}/extend")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ExtendBooking(int bookingId, [FromBody] NewFinishDate date)
    {
        await _bookingsService.ExtendBooking(HttpContext.User.Identity.Name, bookingId, date.FinishAt);

        return NoContent();
    }

    /// <summary>
    /// Конечная точка для отмены бронирования
    /// </summary>
    /// <param name="bookingId">Идентификатор бронирования</param>
    /// <response code="204">Успешная отмена бронирования</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpDelete("{bookingId}")]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        await _bookingsService.CancelBooking(HttpContext.User.Identity.Name, bookingId);

        return NoContent();
    }
}