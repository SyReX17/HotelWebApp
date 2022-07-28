namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если добавляется пользователь, который уже существует
/// </summary>
public class UserExistsException : Exception
{
    public UserExistsException(string message) : base(message) {}
}