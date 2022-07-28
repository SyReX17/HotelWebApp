namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если комната не найдена
/// </summary>
public class RoomSearchException : Exception
{
    public RoomSearchException(string message) : base(message) {}
}