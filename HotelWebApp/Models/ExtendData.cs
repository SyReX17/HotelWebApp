namespace HotelWebApp.Models;

/// <summary>
/// Модель данных для продления проживания пользователя
/// </summary>
public class ExtendData
{
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    public int BookingId { get; set; }
    
    /// <summary>
    /// Новая дата окончания проживания
    /// </summary>
    public DateTime newFinishAt { get; set; }
}