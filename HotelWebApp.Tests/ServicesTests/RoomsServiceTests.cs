using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using HotelWebApp.Services;
using Moq;

namespace HotelWebApp.Tests.ServicesTests;

/// <summary>
/// Класс для тестирования сервиса для работы с комнатами
/// </summary>
public class RoomsServiceTests
{
    /// <summary>
    /// Тестовый список комнат
    /// </summary>
    private List<HotelRoom> _testData;
    
    /// <summary>
    /// Метод для установки начальных значений
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _testData = new List<HotelRoom>
        {
            new HotelRoom { Number = 201, Status = HotelRoomStatus.Free, Type = new HotelRoomType { Name = "Middle", Price = 299}},
            new HotelRoom { Number = 302, Status = HotelRoomStatus.Free, Type = new HotelRoomType { Name = "High", Price = 499}}
        };
    }

    /// <summary>
    /// Метод для тестирования получения всех комнат
    /// </summary>
    [Test]
    public async Task GetAllTests()
    {
        var filter = new RoomFilter();

        var mock = new Mock<IRoomsRepository>();
        mock.Setup(repo => repo.GetAll(filter)).ReturnsAsync(_testData);

        var service = new RoomsService(mock.Object);

        var result = await service.GetAll(filter);
        
        Assert.IsNotNull(result);
        Assert.IsTrue(await RoomListsAreEqual(result, _testData));
    }

    /// <summary>
    /// Метод для тестирования получения команты по идентификатору
    /// </summary>
    [Test]
    public async Task GetRoomByIdTests()
    {
        var mock = new Mock<IRoomsRepository>();
        mock.Setup(repo => repo.GetById(1)).ReturnsAsync(_testData[0]);

        var service = new RoomsService(mock.Object);

        var result = await service.GetById(1);
        
        Assert.IsNotNull(result);
        Assert.IsTrue(await RoomsAreEqual(result, _testData[0]));
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
        var mock = new Mock<IRoomsRepository>();
        mock.Setup(repo => repo.GetFreeRooms(testFilter)).ReturnsAsync(_testData);

        var service = new RoomsService(mock.Object);

        var result = await service.GetFreeRooms(testFilter);
        
        Assert.IsNotNull(result);
        
        Assert.IsTrue(await RoomListsAreEqual(result, _testData));
        
        var errorFilterData = new BookingFilter
        {
            Type = RoomType.High,
            StartAt = DateTime.Now.AddMinutes(10),
            FinishAt = DateTime.Now.AddMinutes(5)
        };

        try
        {
            result = await service.GetFreeRooms(errorFilterData);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is DatesValidationException);
        }
    }

    
    /// <summary>
    /// Метод для проверки эквивалентности списков комнат
    /// </summary>
    /// <param name="a">Первый список</param>
    /// <param name="b">Второй список</param>
    /// <returns>Возваращает true, если списки одинаковы, false, если нет</returns>
    public async Task<bool> RoomListsAreEqual(List<HotelRoom> a, List <HotelRoom> b)
    {
        for (int i = 0; i < b.Count; i++)
        {
            if (!await RoomsAreEqual(a[i], b[i])) return false;
        }

        return true;
    }
    
    /// <summary>
    /// Метод для эквивалентности комнат
    /// </summary>
    /// <param name="a">Первая комната</param>
    /// <param name="b">Вторая комната</param>
    /// <returns>Возваращает true, если комнаты одинаковы, false, если нет</returns>
    public async Task<bool> RoomsAreEqual(HotelRoom a, HotelRoom b)
    {
        if (!(a.Id == b.Id &&
              a.Number == b.Number &&
              a.Status == b.Status &&
              a.Type == b.Type)
           ) return false;
        
        return true;
    }
}