namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если комната не найдена
/// </summary>
public class RoomNotFoundException : RequestException
{
    public RoomNotFoundException(string message, int statusCode) : base(message, statusCode) {}
}