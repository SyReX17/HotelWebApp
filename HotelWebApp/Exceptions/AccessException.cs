namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение, вызываемое при ошибке доступа
/// </summary>
public class AccessException : Exception
{
    public AccessException(string message) : base(message) {}
}