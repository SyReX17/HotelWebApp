using HotelWebApp.Exceptions;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using HotelWebApp.Filters;

namespace HotelWebApp.Controllers;

/// <summary>
/// Контроллер для работы с комнатами, включает набор
/// конечных точек для получения комнат
/// </summary>
[ApiController]
[Route("api/rooms")]
[Authorize(Roles = "Admin, User")]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    /// <summary>
    /// Реализация репозитория для работы с БД
    /// через интерфейс <c>IRoomRepository</c>
    /// </summary>
    private IRoomRepository _roomsRepository;

    /// <summary>
    /// Конструктор контроллера, устанавливает класс,
    /// реализующий интерфейс репозитория
    /// </summary>
    public RoomsController(IRoomRepository roomRepository)
    {
        this._roomsRepository = roomRepository;
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
        var rooms = await _roomsRepository.GetAll(filter);

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
        var room = await _roomsRepository.GetById(id);

        if (room == null) throw new RoomNotFoundException();

        return Ok(room);
    }
}