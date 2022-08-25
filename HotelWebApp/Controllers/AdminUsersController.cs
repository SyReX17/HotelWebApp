using HotelWebApp.Filters;
using HotelWebApp.Interfaces.Services;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers;

/// <summary>
/// Класс контроллера для работы администратора с пользователями
/// </summary>
[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AdminUsersController : ControllerBase
{
    /// <summary>
    /// Интерфейс сервиса для работы с пользователями
    /// </summary>
    private readonly IUsersService _usersService;

    /// <summary>
    /// Конструктор контроллера, устанавливает класс,
    /// реализующий интерфейс сервиса
    /// </summary>
    /// <param name="usersService">Сервис для работы с пользователями</param>
    public AdminUsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }
    
    /// <summary>
    /// Конечная точка для получения списка пользователей с
    /// использованием фильтров и сортировки
    /// </summary>
    /// <param name="filter">Фильтр для пользователей</param>
    /// <response code="200">Успешное получение всех пользователей</response>
    /// <response code="400">Данные введены некоректно</response>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<UserDTO>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var users = await _usersService.GetAll(filter);

        return Ok(users);
    }
}