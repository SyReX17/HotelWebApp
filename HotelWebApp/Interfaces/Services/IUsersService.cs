using System.Security.Claims;
using HotelWebApp.Filters;
using HotelWebApp.Models;

namespace HotelWebApp.Interfaces.Services;

/// <summary>
/// Интерфейс сервиса для работы с пользователями
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Получение всех пользователей
    /// </summary>
    /// <param name="filter">Фильтр пользователей</param>
    /// <returns>Список пользователей в виде объектов <c>UserDTO</c></returns>
    Task<List<UserDTO>> GetAll(UserFilter filter);

    /// <summary>
    /// Получения списка клеймов пользователя
    /// </summary>
    /// <param name="loginData">Информация для логина пользователя, включает email и пароль</param>
    /// <returns>Список клеймов пользователя</returns>
    Task<List<Claim>> GetUserClaims(LoginData loginData);

    /// <summary>
    /// Добавление нового пользователя
    /// </summary>
    /// <param name="registerData">Регистрационные ланные пользователя, включают email и пароль</param>
    Task Add(RegisterData registerData);

    /// <summary>
    /// Получение пользователя по email
    /// </summary>
    /// <param name="email">Email пользователя</param>
    /// <returns>Пользователь в виде объекта <c>User</c></returns>
    Task<User> GetByEmail(string email);
}