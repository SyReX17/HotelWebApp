namespace HotelWebApp.Enums;

/// <summary>
/// Статусы оплаты
/// </summary>
public enum InvoiceStatus : byte
{
    /// <summary>
    /// Подтверждена
    /// </summary>
    Confirm,
    
    /// <summary>
    /// Ожидает подтверждения
    /// </summary>
    Awaiting
}