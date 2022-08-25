using HotelWebApp.Enums;
using HotelWebApp.Models;

namespace HotelWebApp.Tests.ModelTests;

/// <summary>
/// Класс для тестирования модели счета на оплату
/// </summary>
public class InvoiceModelTests
{
    /// <summary>
    /// Метод для тестирования подтверждения оплаты
    /// </summary>
    [Test]
    public async Task ConfirmTests()
    {
        var invoice = new Invoice
        {
            Id = 1,
            ResidentId = 2,
            ResidentName = "Пользватель",
            RoomId = 1,
            Price = 199,
            BookingId = 1,
            Status = InvoiceStatus.Awaiting
        };
        
        invoice.Confirm();
        
        Assert.AreEqual(invoice.Status, InvoiceStatus.Confirm);
    }
}