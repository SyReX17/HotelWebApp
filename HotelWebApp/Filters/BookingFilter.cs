using System.ComponentModel.DataAnnotations;
using HotelWebApp.Enums;

namespace HotelWebApp.Filters;

/// <summary>
/// Фильтр для получения свободных комнат
/// </summary>
public class BookingFilter
{
    /// <summary>
    /// Тип комнаты
    /// </summary>
    [Required]
    public RoomType Type { get; set; }
    
    /// <summary>
    /// Дата начала проживания
    /// </summary>
    [Required]
    public DateTime StartAt { get; set; }
    
    /// <summary>
    /// Дата окончания проживания
    /// </summary>
    [Required]
    public DateTime FinishAt { get; set; }
}