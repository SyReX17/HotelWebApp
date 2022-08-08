using HotelWebApp.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelWebApp.Repositories;

namespace HotelWebApp.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class AdminController : ControllerBase
{
    /// <summary>
    /// Реализация репозитория для работы с БД
    /// через интерфейс <c>IUserRepository</c>
    /// </summary>
    private IUserRepository _usersRepository;

    /// <summary>
    /// Конструктор контроллера, устанавливает класс,
    /// реализующий интерфейс репозитория
    /// </summary>
    public AdminController(IUserRepository userRepository)
    {
        this._usersRepository = userRepository;
    }
    
    /// <summary>
    /// Конечная точка для получения списка пользователей с
    /// использованием фильтров и сортировки
    /// </summary>
    /// <param name="filter">Фильтр для пользователей</param>
    /// <response code="200">Успешное получение всех пользователей</response>
    /// <response code="400">Данные введены некоректно</response>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<User>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var users = await _usersRepository.GetAll(filter);

        return Ok(users);
    }
}