using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using HotelWebApp.Services;
using Moq;

namespace HotelWebApp.Tests.ServicesTests;

/// <summary>
/// Класс для тестирования сервиса для работы с счетами на оплату
/// </summary>
public class InvoicesServiceTests
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
        var mock = new Mock<IInvoicesRepository>();
        mock.Setup(repo => repo.GetInvoices()).ReturnsAsync(_testData);

        var service = new InvoicesService(mock.Object);

        var result = await service.GetInvoices();
        
        Assert.IsNotNull(result);
        Assert.IsTrue(await InvoicesListsIsEqual(result, _testData));
    }

    /// <summary>
    /// Метод для тестирования подтверждения оплаты
    /// </summary>
    [Test]
    public async Task ConfirmInvoiceTests()
    {
        Invoice? nullInvoice = null;
        var mock = new Mock<IInvoicesRepository>();
        mock.Setup(repo => repo.GetInvoiceById(1)).ReturnsAsync(nullInvoice);

        var service = new InvoicesService(mock.Object);

        try
        {
            await service.ConfirmInvoice(1);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is InvoiceNotFoundException);
        }
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