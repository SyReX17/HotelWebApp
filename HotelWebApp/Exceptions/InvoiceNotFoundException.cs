namespace HotelWebApp.Exceptions;

/// <summary>
/// Исключение, вызываемое при отсутствии счета на оплату, наследуется от RequestException
/// </summary>
public class InvoiceNotFoundException : RequestException
{
    public InvoiceNotFoundException() : base("Счет на оплату не найден", 400) {}
}