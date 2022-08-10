using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
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
    /// Реализация репозитория для работы с пользователями
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Реализация репозитория для работы с бронями
    /// </summary>
    private readonly IBookingRepository _bookingRepository;

    /// <summary>
    /// Конструктор контроллера, устанавливает классы
    /// реализующие интерфейсы репозиториев
    /// </summary>
    public BookingsController(IUserRepository userRepository, IBookingRepository bookingRepository)
    {
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
    }
    
    /// <summary>
    /// Конечная точка для добавления новой брони
    /// </summary>
    /// <param name="bookingData">Данные о новой брони полученные из тела запроса</param>
    /// <exception cref="DatesValidationException">Даты введены неверно</exception>
    /// <response code="200">Успешное добавление брони</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> AddBooking([FromBody] UserBookingData bookingData)
    {
        if (bookingData.StartAt >= bookingData.FinishAt || bookingData.StartAt < DateTime.Now)
        {
            throw new DatesValidationException();
        }
        
        var booking = new Booking
        {
            ResidentId = await _userRepository.GetByEmail(HttpContext.User.Identity.Name),
            RoomId = bookingData.RoomId,
            Status = BookingStatus.Awaiting,
            StartAt = bookingData.StartAt,
            FinishAt = bookingData.FinishAt
        };

        await _bookingRepository.Add(booking);

        return Ok();
    }

    /// <summary>
    /// Конечная точка для получения свободных комнат
    /// </summary>
    /// <param name="filter">Фильтр для получения свободных комнат</param>
    /// <exception cref="DatesValidationException">Даты введены неверно</exception>
    /// <response code="200">Успешное получение свободных комнат</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(List<HotelRoom>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetFreeRooms([FromQuery] BookingFilter filter)
    {
        if (filter.StartAt >= filter.FinishAt || filter.StartAt < DateTime.Now)
        {
            throw new DatesValidationException();
        }
        
        var freeRooms = await _bookingRepository.GetFreeRooms(filter);
        return Ok(freeRooms);
    }

    /// <summary>
    /// Конечная точка для продления бронирования
    /// </summary>
    /// <param name="extendData">Данные для прдления бронирования</param>
    /// <response code="200">Успешное продление бронирования</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut]
    [Authorize]
    [Route("{bookingId}/extend")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ExtendBooking(int bookingId, [FromBody] DateTime newFinishAt)
    {
        var userId = await _userRepository.GetByEmail(HttpContext.User.Identity.Name);
        await _bookingRepository.ExtendBooking(userId, bookingId, newFinishAt);

        return NoContent();
    }

    /// <summary>
    /// Конечная точка для отмены бронирования
    /// </summary>
    /// <param name="bookingId">Идентификатор бронирования</param>
    /// <response code="200">Успешная отмена бронирования</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpDelete("{bookingId}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        var userId = await _userRepository.GetByEmail(HttpContext.User.Identity.Name);
        await _bookingRepository.RemoveBooking(userId, bookingId);

        return NoContent();
    }
}