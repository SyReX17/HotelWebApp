namespace HotelWebApp.Exceptions;

public class UnauthorizedException : RequestException
{
    public UnauthorizedException() : base("Пользователь не авторизован", 401) {}
}