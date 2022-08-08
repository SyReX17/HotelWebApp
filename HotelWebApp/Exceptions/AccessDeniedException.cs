namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение, вызываемое при ошибке доступа, наследуется от RequestException
/// </summary>
public class AccessDeniedException : RequestException
{
    public AccessDeniedException(string message, int statusCode) : base(message, statusCode) {}
}