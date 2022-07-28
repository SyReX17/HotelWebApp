namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если пользователь отсутсвует в БД
/// </summary>
public class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message) {}
}