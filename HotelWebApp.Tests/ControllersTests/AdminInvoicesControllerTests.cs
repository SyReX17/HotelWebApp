using HotelWebApp.Controllers;
using HotelWebApp.Enums;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelWebApp.Tests;

/// <summary>
/// Класс для тестирования контроллера администатора для работы с счетами на оплату
/// </summary>
public class AdminInvoicesControllerTests
{
    /// <summary>
    /// Тестовые данные
    /// </summary>
    private List<Invoice> _testData;
    
    /// <summary>
    /// Метод для установки начальных значений
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _testData = new List<Invoice>
        {
            new Invoice
            {
                Id = 1,
                ResidentId = 2,
                ResidentName = "Пользватель",
                RoomId = 1,
                Price = 199,
                BookingId = 1,
                Status = InvoiceStatus.Awaiting
            },
            new Invoice
            {
                Id = 2,
                ResidentId = 1,
                ResidentName = "Тест",
                RoomId = 2,
                Price = 299,
                BookingId = 2,
                Status = InvoiceStatus.Awaiting
            }
        };
    }

    /// <summary>
    /// Метод для тестирования получения счетов на оплату
    /// </summary>
    [Test]
    public async Task GetInvoicesTests()
    {
        var mock = new Mock<IBookingRepository>();
        mock.Setup(repo => repo.GetInvoices()).ReturnsAsync(_testData);

        var controller = new AdminInvoicesController(mock.Object);

        var result = await controller.GetInvoices() as OkObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        var listResult = result.Value as List<Invoice>;
        
        Assert.IsTrue(await InvoicesListsIsEqual(_testData, listResult));
    }

    /// <summary>
    /// Метод для тестирования подтверждения счета на оплату
    /// </summary>
    [Test]
    public async Task ConfirmInvoiceTests()
    {
        var mock = new Mock<IBookingRepository>();
        mock.Setup(repo => repo.ConfirmInvoice(1));

        var controller = new AdminInvoicesController(mock.Object);

        var result = await controller.ConfirmInvoice(1) as NoContentResult;
        
        Assert.AreEqual(StatusCodes.Status204NoContent, result.StatusCode);
    }

    /// <summary>
    /// Метод для проверки эквивалентности списков счетов на оплату
    /// </summary>
    /// <param name="a">Первый список</param>
    /// <param name="b">Второй список</param>
    /// <returns>Возваращает true, если списки одинаковы, false, если нет</returns>
    public async Task<bool> InvoicesListsIsEqual(List<Invoice> a, List<Invoice> b)
    {
        for (int i = 0; i < b.Count; i++)
        {
            if (!(a[i].Id == b[i].Id && a[i].ResidentId == b[i].ResidentId && a[i].BookingId == b[i].BookingId &&
                  a[i].ResidentName == b[i].ResidentName && a[i].Price == b[i].Price && a[i].RoomId == b[i].RoomId &&
                  a[i].Status == b[i].Status)) return false;
        }

        return true;
    }
}