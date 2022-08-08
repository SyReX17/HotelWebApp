using System.ComponentModel.DataAnnotations;
using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HotelWebApp.Controllers;

/// <summary>
/// Контроллер для работы с бронированием комнат
/// </summary>
[ApiController]
[Route("api/booking")]
[Produces("application/json")]
public class BookingController : ControllerBase
{
    /// <summary>
    /// Реализация репозитория для работы с комнатами
    /// </summary>
    private IRoomRepository _roomRepository;

    /// <summary>
    /// Реализация репозитория для работы с пользователями
    /// </summary>
    private IUserRepository _userRepository;

    /// <summary>
    /// Реализация репозитория для работы с бронями
    /// </summary>
    private IBookingRepository _bookingRepository;

    /// <summary>
    /// Конструктор контроллера, устанавливает классы
    /// реализующие интерфейсы репозиториев
    /// </summary>
    public BookingController(IRoomRepository roomRepository, IUserRepository userRepository, IBookingRepository bookingRepository)
    {
        this._roomRepository = roomRepository;
        this._userRepository = userRepository;
        this._bookingRepository = bookingRepository;
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
    public async Task<IActionResult> BookRoom([FromBody] UserBookingData bookingData)
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
    /// Конечная точка для подтверждения бронирования администратором
    /// </summary>
    /// <param name="bookingId">Идентификатор бронирования</param>
    /// <response code="200">Успешное подтверждения брони</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut("{bookingId}")]
    [Authorize(Roles = "Admin")]
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
        return Ok();
    }

    /// <summary>
    /// Конечная точка для получения всех броней
    /// </summary>
    /// <response code="200">Успешное получение всех броней</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(List<Booking>))]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingRepository.GetAll();

        return Ok(bookings);
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
    [Route("free")]
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
    /// Конечная точка для получения счетов на оплату
    /// </summary>
    /// <response code="200">Успешное получение счетов на оплату</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("invoices")]
    [ProducesResponseType(200, Type = typeof(List<Invoice>))]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetInvoices()
    {
        var invoices = await _bookingRepository.GetInvoices();
        return Ok(invoices);
    }

    /// <summary>
    /// Конечная точка для подтверждения оплаты
    /// </summary>
    /// <param name="invoiceId">Идентификатор счета на оплату</param>
    /// <response code="200">Успешное подтверждение оплаты</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("invoices/{invoiceId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ConfirmInvoice(int invoiceId)
    {
        await _bookingRepository.ConfirmInvoice(invoiceId);
        return Ok();
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
    [Route("extend")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ExtendBooking([FromQuery] ExtendData extendData)
    {
        var userId = await _userRepository.GetByEmail(HttpContext.User.Identity.Name);
        await _bookingRepository.ExtendBooking(userId, extendData.BookingId, extendData.newFinishAt);
        
        return Ok();
    }

    /// <summary>
    /// Конечная точка для отмены бронирования
    /// </summary>
    /// <param name="bookingId">Идентификатор бронирования</param>
    /// <response code="200">Успешная отмена бронирования</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpDelete]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> CancelBooking([FromQuery] int bookingId)
    {
        var userId = await _userRepository.GetByEmail(HttpContext.User.Identity.Name);
        await _bookingRepository.RemoveBooking(userId, bookingId);

        return Ok();
    }

    /// <summary>
    /// Конечная точка для выселения клиента
    /// </summary>
    /// <param name="userId">Идентификатор клиента</param>
    /// <response code="200">Успешное выселение клиента</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("evict/{userId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> EvictClient(int userId)
    {
        await _bookingRepository.EvictClient(userId);
        return Ok();
    }
}