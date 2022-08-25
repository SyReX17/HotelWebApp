namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение, вызываемое, если пользователь не авторизован, наследуется от RequestException
/// </summary>
public class UnauthorizedException : RequestException
{
    public UnauthorizedException() : base("Пользователь не авторизован", 401) {}
}