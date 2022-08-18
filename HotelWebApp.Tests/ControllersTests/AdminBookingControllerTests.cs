using HotelWebApp.Controllers;
using HotelWebApp.Enums;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelWebApp.Tests;

/// <summary>
/// Класс для тестирования контроллера администратора для работы с бронями
/// </summary>
public class AdminBookingControllerTests
{
    /// <summary>
    /// Тестовый список с данными о бронировании
    /// </summary>
    private List<Booking> _testData;
    
    /// <summary>
    /// Метод для установки начальных значений
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _testData = new List<Booking>
        {
            new Booking
            {
                Id = 1,
                ResidentId = 2,
                RoomId = 1,
                Status = BookingStatus.Awaiting,
                StartAt = DateTime.Now.AddMinutes(5),
                FinishAt = DateTime.Now.AddMinutes(10),
            },
            new Booking
            {
                Id = 2,
                ResidentId = 1,
                RoomId = 1,
                Status = BookingStatus.Awaiting,
                StartAt = DateTime.Now.AddMinutes(5),
                FinishAt = DateTime.Now.AddMinutes(10),
            }
        };
    }

    /// <summary>
    /// Метод для ткстирования эндпоинта для получения всех комнат,
    /// проверяет корректность результата, полученного из контроллера
    /// </summary>
    [Test]
    public async Task GetAllBookingsTest()
    {
        var mock = new Mock<IBookingsRepository>();
        mock.Setup(repo => repo.GetAll()).ReturnsAsync(_testData);
        var controller = new AdminBookingsController(mock.Object);

        var result = await controller.GetAllBookings() as OkObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        var listResult = result.Value as List<Booking>;
        
        Assert.IsTrue(await BookingListIsEqual(_testData, listResult));
    }

    /// <summary>
    /// Метод для тестироавния эндпоинта для подтверждения юронироавния комнаты
    /// </summary>
    [Test]
    public async Task ConfirmBookingTest()
    {
        var newStatusData = new StatusData
        {
            BookingId = 1,
            NewStatus = BookingStatus.Confirm
        };
        var mock = new Mock<IBookingsRepository>();
        mock.Setup(repo => repo.UpdateStatus(newStatusData));
        var controller = new AdminBookingsController(mock.Object);

        var result = await controller.ConfirmBooking(1) as NoContentResult;
        
        Assert.AreEqual(StatusCodes.Status204NoContent, result.StatusCode);
    }

    /// <summary>
    /// Метод для тестирования эндпоинта для выселения клиента из комнаты
    /// </summary>
    [Test]
    public async Task EvictClientTest()
    {
        var mock = new Mock<IBookingsRepository>();
        mock.Setup(repo => repo.EvictClient(1));
        var controller = new AdminBookingsController(mock.Object);

        var result = await controller.EvictClient(1) as NoContentResult;
        
        Assert.AreEqual(StatusCodes.Status204NoContent, result.StatusCode);
    }

    /// <summary>
    /// Метод для проверки эквивалентности значений списка
    /// </summary>
     
    public async Task<bool> BookingListIsEqual(List<Booking> a, List<Booking> b)
    {
        for (int i = 0; i < b.Count; i++)
        {
            if (!(a[i].Id == b[i].Id && a[i].ResidentId == b[i].ResidentId && a[i].RoomId == b[i].RoomId &&
                  a[i].Status == b[i].Status && a[i].StartAt == b[i].StartAt && a[i].FinishAt == b[i].FinishAt))
            {
                return false;
            }
        }

        return true;
    }
}