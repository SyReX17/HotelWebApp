using HotelWebApp.Filters;
using HotelWebApp.Mappers;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelWebApp.Repositories;

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
    /// Реализация репозитория для работы с БД
    /// через интерфейс <c>IUserRepository</c>
    /// </summary>
    private readonly IUserRepository _usersRepository;

    /// <summary>
    /// Конструктор контроллера, устанавливает класс,
    /// реализующий интерфейс репозитория
    /// </summary>
    public AdminUsersController(IUserRepository userRepository)
    {
        _usersRepository = userRepository;
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
        var users = await _usersRepository.GetAll(filter);

        return Ok(users.Select(u => Mapper.ToUserDTO(u)).ToList());
    }
}