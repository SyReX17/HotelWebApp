using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelWebApp.Models;

/// <summary>
/// Класс для получения новой даты окнчания проживания
/// </summary>
public class NewFinishDate
{
    /// <summary>
    /// Новая дата окончания проживания
    /// </summary>
    [Required]
    [JsonPropertyName("finishAt")]
    public DateTime FinishAt { get; set; }
}