using HotelWebApp.Filters;
using HotelWebApp.Models;

namespace HotelWebApp.Repositories;

/// <summary>
/// Интерфейс для работы с репозиторием комнат
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Добавляет нового пользователя
    /// </summary>
    /// <param name="loginData">
    /// Email и пароль пользователя
    /// </param>
    Task Add(RegisterData registerData);

    /// <summary>
    /// Возвращает список комнат с использованием фильтров и сортировки
    /// </summary>
    /// <param name="filter">Фильтр для поиска пользователей</param>
    /// <returns>Список пользователей</returns>
    Task<List<User>> GetAll(UserFilter filter);

    /// <summary>
    /// Возвращает пользователя по id
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Пользователя в виде объекта <c>User</c></returns>
    Task<User?> GetById(int id);

    /// <summary>
    /// Возвращает пользователя по email
    /// </summary>
    /// <param name="email">Email пользователя</param>
    /// <returns>Пользователя в виде объекта <c>User</c></returns>
    Task<User?> GetByEmail(string email);
}