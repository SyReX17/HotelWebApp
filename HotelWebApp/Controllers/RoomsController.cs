using HotelWebApp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using HotelWebApp.Filters;
using HotelWebApp.Interfaces.Services;

namespace HotelWebApp.Controllers;

/// <summary>
/// Контроллер для работы с комнатами, включает набор
/// конечных точек для получения комнат
/// </summary>
[ApiController]
[Route("api/rooms")]
[Authorize]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    /// <summary>
    /// Интерфейс сервиса для работы с комнатами
    /// </summary>
    private readonly IRoomsService _roomsService;

    /// <summary>
    /// Конструктор контроллера, устанавливает класс,
    /// реализующий интерфейс сервиса
    /// </summary>
    /// <param name="roomsService">Сервис для работы с комнатами</param>
    public RoomsController(IRoomsService roomsService)
    {
        _roomsService = roomsService;
    }
    
    /// <summary>
    /// Конечная точка получения списка комнат с
    /// использованием фильтров и сортировки
    /// </summary>
    /// <param name="filter">Фильтр для комнат</param>
    /// <response code="200">Успешное получение комнат</response>
    /// <response code="400">Данные введены некоректно</response> 
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<HotelRoom>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetAllRooms([FromQuery] RoomFilter filter)
    {
        var rooms = await _roomsService.GetAll(filter);

        return Ok(rooms);
    }

    /// <summary>
    /// Конечная точка для получения комнат по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор комнаты</param>
    /// Объект комнаты и статусный код Ок(200), или генерирует исключение
    /// <response code="200">Успешное получение комнаты</response>
    /// <response code="400">Данные введены некоректно</response>
    /// <response code="403">Отсутствие доступа к ресурсу</response>
    /// <exception cref="RoomSearchException">Комната не найдена</exception>
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(HotelRoom))]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetRoom(int id)
    {
        var room = await _roomsService.GetById(id);

        return Ok(room);
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
    [Route("free")]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(List<HotelRoom>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetFreeRooms([FromQuery] BookingFilter filter)
    {
        var freeRooms = await _roomsService.GetFreeRooms(filter);
        
        return Ok(freeRooms);
    }
}