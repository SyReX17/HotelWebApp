using System.Net;
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
/// Класс для тестирования контроллера для работы с комнатами
/// </summary>
public class RoomsControllerTests
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
    /// Метод для проверки получения всех комнат
    /// </summary>
    [Test]
    public async Task GetAllRoomsTests()
    {
        var filter = new RoomFilter();

        var mock = new Mock<IRoomRepository>();
        mock.Setup(repo => repo.GetAll(filter)).ReturnsAsync(_testData);

        var controller = new RoomsController(mock.Object);

        var result = await controller.GetAllRooms(filter) as OkObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        var listResult = result.Value as List<HotelRoom>;
        
        Assert.IsTrue(await RoomsListsAreEqual(_testData, listResult));
    }

    /// <summary>
    /// Метод для проверки получения комнаты по идентификатору
    /// </summary>
    [Test]
    public async Task GetRoomByIdTests()
    {
        var mock = new Mock<IRoomRepository>();
        mock.Setup(repo => repo.GetById(1)).ReturnsAsync(_testData[0]);

        var controller = new RoomsController(mock.Object);

        var result = await controller.GetRoom(1) as OkObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        var roomResult = result.Value as HotelRoom;
        
        Assert.IsTrue(await RoomsAreEqual(_testData[0], roomResult));
    }
    
    /// <summary>
    /// Метод для проверки эквивалентности списков комнат
    /// </summary>
    /// <param name="a">Первый список</param>
    /// <param name="b">Второй список</param>
    /// <returns>Возваращает true, если списки одинаковы, false, если нет</returns>
    public async Task<bool> RoomsListsAreEqual(List<HotelRoom> a, List<HotelRoom> b)
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