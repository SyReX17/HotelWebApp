using HotelWebApp.Enums;

namespace HotelWebApp.Models;

/// <summary>
/// Модель данных для счета на оплату
/// </summary>
public class Invoice
{
    /// <summary>
    /// Идентификатор счета на оплату
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Статус оплаты
    /// </summary>
    public InvoiceStatus Status { get; set; }
    
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    public int BookingId { get; set; }
    
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public int ResidentId { get; set; }
    
    /// <summary>
    /// Полное имя клиента
    /// </summary>
    public string ResidentName { get; set; }
    
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public int RoomId { get; set; }
    
    /// <summary>
    /// Стоимость проживания
    /// </summary>
    public decimal Price { get; set; }
}