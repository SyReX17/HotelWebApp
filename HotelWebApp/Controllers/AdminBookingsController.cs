using HotelWebApp.Enums;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;

[ApiController]
[Route("api/admin/bookings")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AdminBookingsController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;

    public AdminBookingsController(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
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
        var bookings = await _bookingRepository.GetAll();

        return Ok(bookings);
    }
    
    /// <summary>
    /// Конечная точка для подтверждения бронирования администратором
    /// </summary>
    /// <param name="bookingId">Идентификатор бронирования</param>
    /// <response code="200">Успешное подтверждения брони</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut("{bookingId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ConfirmBooking(int bookingId)
    {
        var statusData = new StatusData
        {
            BookingId = bookingId,
            NewStatus = BookingStatus.Confirm
        };
        await _bookingRepository.UpdateStatus(statusData);
        return NoContent();
    }
    
    /// <summary>
    /// Конечная точка для выселения клиента
    /// </summary>
    /// <param name="userId">Идентификатор клиента</param>
    /// <response code="200">Успешное выселение клиента</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut]
    [Route("{userId}/evict")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> EvictClient(int bookingId)
    {
        await _bookingRepository.EvictClient(bookingId);
        return NoContent();
    }
}