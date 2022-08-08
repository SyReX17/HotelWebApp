using HotelWebApp.Enums;

namespace HotelWebApp.Models;

/// <summary>
/// Модель для изменения статуса бронирования
/// </summary>
public class StatusData
{
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    public int BookingId { get; set; }
    
    /// <summary>
    /// Статус бронирования
    /// </summary>
    public BookingStatus NewStatus { get; set; }
}