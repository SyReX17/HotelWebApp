namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если пользователь отсутсвует в БД
/// </summary>
public class UserNotFoundException : RequestException
{
    public UserNotFoundException(string message, int statusCode) : base(message, statusCode) {}
}