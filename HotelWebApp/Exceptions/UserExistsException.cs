namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если добавляется пользователь, который уже существует
/// </summary>
public class UserExistsException : RequestException
{
    public UserExistsException(string message, int statusCode) : base(message, statusCode) {}
}