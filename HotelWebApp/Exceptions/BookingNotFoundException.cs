namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение, вызываемое при отсутствии брони, наследуется от RequestException
/// </summary>
public class BookingNotFoundException : RequestException
{
    public BookingNotFoundException() : base("Бронироавние не найдено", 400) {}
}