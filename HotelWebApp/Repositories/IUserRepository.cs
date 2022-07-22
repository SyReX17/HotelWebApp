namespace HotelWebApp.Repositories;

/// <summary>
/// Интерфейс для работы с репозиторием
/// </summary>
public interface IUserRepository : IDisposable
{
    /// <summary>
    /// Возвражает пользовалеля по его данным
    /// </summary>
    /// <param name="loginData">
    /// Email и пароль пользователя
    /// </param>
    /// <returns>
    /// Пользователя в виде объекта <c>User</c>
    /// </returns>
    Task<User?> Get(LoginData loginData);
    
    /// <summary>
    /// Добавляет нового пользователя
    /// </summary>
    /// <param name="loginData">
    /// Email и пароль пользователя
    /// </param>
    Task Add(LoginData loginData);
}