namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если добавляется пользователь, который уже существует, наследуется от RequestException
/// </summary>
public class UserExistsException : RequestException
{
    public UserExistsException() : base("Такой пользователь уже существует", 400) {}
}