using HotelWebApp.Controllers;
using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelWebApp.Tests;

/// <summary>
/// Класс для тестирования контроллера для работы с бронями
/// </summary>
public class BookingControllerTests
{
    /// <summary>
    /// Тестовый пользователь
    /// </summary>
    private User _testUser;
    
    /// <summary>
    /// Тестовый список комнат
    /// </summary>
    private List<HotelRoom> _testRooms; 
    
    /// <summary>
    /// Метод для установки начальных значений
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _testUser = new User
        {
            Id = 2,
            Email = "user@mail.ru",
            Password = "55555",
            FullName = "Пользователь",
            RegisteredAt = DateTime.Today,
            Role = Role.User
        };

        _testRooms = new List<HotelRoom>
        {
            new HotelRoom { Number = 302, Status = HotelRoomStatus.Free, Type = new HotelRoomType { Name = "High", Price = 499}}
        };
    }

    /// <summary>
    /// Метод для проверки создания брони и проверки ошибок при создании брони
    /// </summary>
    [Test]
    public async Task CreateBookingTests()
    {
        var errorTestData = new UserBookingData
        {
            RoomId = 1,
            StartAt = DateTime.Now.AddMinutes(10),
            FinishAt = DateTime.Now.AddMinutes(5)
        };
        
        var testBooking = new Booking
        {
            ResidentId = 2,
            RoomId = 1,
            Status = BookingStatus.Awaiting,
            StartAt = errorTestData.FinishAt,
            FinishAt = errorTestData.StartAt
        };

        
        var bookingsRepositoryMock = new Mock<IBookingsRepository>();
        var usersRepositoryMock = new Mock<IUsersRepository>();
        usersRepositoryMock.Setup(repo => repo.GetByEmail(_testUser.Email)).ReturnsAsync(_testUser);
        bookingsRepositoryMock.Setup(repo => repo.Add(testBooking));

        var controller = new BookingsController(usersRepositoryMock.Object, bookingsRepositoryMock.Object);

        try
        {
            var result = await controller.AddBooking(errorTestData) as NoContentResult;
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is DatesValidationException);
        }
    }

    /// <summary>
    /// Метод для проверки получения всех комнат и ошибок при получении комнат
    /// </summary>
    [Test]
    public async Task GetFreeRoomsTests()
    {
        var testFilter = new BookingFilter
        {
            Type = RoomType.High,
            StartAt = DateTime.Now.AddMinutes(5),
            FinishAt = DateTime.Now.AddMinutes(10)
        };
        var bookingsRepositoryMock = new Mock<IBookingsRepository>();
        var usersRepositoryMock = new Mock<IUsersRepository>();
        bookingsRepositoryMock.Setup(repo => repo.GetFreeRooms(testFilter)).ReturnsAsync(_testRooms);

        var controller = new BookingsController(usersRepositoryMock.Object, bookingsRepositoryMock.Object);

        var result = await controller.GetFreeRooms(testFilter) as OkObjectResult;

        var resultList = result.Value as List<HotelRoom>;
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsTrue(await RoomsListsAreEqual(resultList, _testRooms));
        
        var errorFilterData = new BookingFilter
        {
            Type = RoomType.High,
            StartAt = DateTime.Now.AddMinutes(10),
            FinishAt = DateTime.Now.AddMinutes(5)
        };

        try
        {
            result = await controller.GetFreeRooms(errorFilterData) as OkObjectResult;
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is DatesValidationException);
        }
    }

    /// <summary>
    /// Метод для эквивалентности списков комнат
    /// </summary>
    /// <param name="a">Первый список</param>
    /// <param name="b">Второй список</param>
    /// <returns>Возваращает true, если списки одинаковы, false, если нет</returns>
    public async Task<bool> RoomsListsAreEqual(List<HotelRoom> a, List<HotelRoom> b)
    {
        for (int i = 0; i < b.Count; i++)
        {
            if (!(a[i].Id == b[i].Id &&
                  a[i].Number == b[i].Number &&
                  a[i].Status == b[i].Status &&
                  a[i].Type == b[i].Type)) return false;
        }
        return true;
    }
}