using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelWebApp.Models;

/// <summary>
/// Модель данных для создания нового бронирования
/// </summary>
public class UserBookingData
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    [Required]
    [JsonPropertyName("roomId")]
    public int RoomId { get; set; }
    
    /// <summary>
    /// Дата начала проживания
    /// </summary>
    [Required]
    [JsonPropertyName("startAt")]
    public DateTime StartAt { get; set; }
    
    /// <summary>
    /// Дата окончания проживания
    /// </summary>
    [Required]
    [JsonPropertyName("finishAt")]
    public DateTime FinishAt { get; set; }
}