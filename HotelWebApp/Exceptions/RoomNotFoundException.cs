namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение вызываемое, если комната не найдена, наследуется от RequestException
/// </summary>
public class RoomNotFoundException : RequestException
{
    public RoomNotFoundException() : base("Комната не найдена", 400) {}
}