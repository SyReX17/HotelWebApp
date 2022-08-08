namespace HotelWebApp.Enums;

/// <summary>
/// Статус бронирования комнаты
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// Подтверждено
    /// </summary>
    Confirm,
    
    /// <summary>
    /// Ожидает ответа
    /// </summary>
    Awaiting,
    
    /// <summary>
    /// Отмена бронирования
    /// </summary>
    Deny
}