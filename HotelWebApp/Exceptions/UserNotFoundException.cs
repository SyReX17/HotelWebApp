namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если пользователь отсутсвует в БД, наследуется от RequestException
/// </summary>
public class UserNotFoundException : RequestException
{
    public UserNotFoundException() : base("Пользователь не найден", 401) {}
}