using HotelWebApp.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelWebApp.Repositories;

namespace HotelWebApp.Controllers;

[ApiController]
[Route("api/admin")]
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
    public AdminController()
    {
        this._usersRepository = new UsersRepository();
    }
    
    /// <summary>
    /// Конечная точка для получения списка пользователей с
    /// использованием фильтров и сортировки
    /// </summary>
    /// <param name="filter">Фильтр для пользователей</param>
    /// <response code="200">Успешное получение всех пользователей</response>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<User>))]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var users = await _usersRepository.GetAll(filter);

        return Ok(users);
    }
}