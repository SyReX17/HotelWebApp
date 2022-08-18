using HotelWebApp.Controllers;
using HotelWebApp.Enums;
using HotelWebApp.Filters;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelWebApp.Tests;

/// <summary>
/// Класс для тестирования контроллера администатора для работы с пользователями
/// </summary>
public class AdminUsersControllerTests
{
    /// <summary>
    /// Тестовые данные
    /// </summary>
    private List<User> _testData;
    
    /// <summary>
    /// Метод для установки начальных значений
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _testData = new List<User>
        {
            new User
            {
                Id = 1,
                Email = "admin@mail.ru",
                Password = "12345",
                FullName = "Администратор",
                RegisteredAt = DateTime.Today,
                Role = Role.Admin
            },
            new User
            {
                Id = 2,
                Email = "user@mail.ru",
                Password = "55555",
                FullName = "Пользователь",
                RegisteredAt = DateTime.Today,
                Role = Role.User
            }
        };
    }

    /// <summary>
    /// Метод тестирования получения всех пользователей
    /// </summary>
    [Test]
    public async Task GetAllUsersTest()
    {
        var filter = new UserFilter();
        var mock = new Mock<IUsersRepository>();
        mock.Setup(repo => repo.GetAll(filter)).ReturnsAsync(_testData);
        var controller = new AdminUsersController(mock.Object);

        var testResultList = new List<UserDTO>();

        foreach (var user in _testData)
        {
            testResultList.Add(user.ToUserDTO());
        }

        var result = await controller.GetUsers(filter) as OkObjectResult;
        
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        
        var listResult = result.Value as List<UserDTO>;

        Assert.IsTrue( await UserDTOListsIsEqual(testResultList, listResult));
    }

    /// <summary>
    /// Метод для проверки эквивалентности списков пользователей
    /// </summary>
    /// <param name="a">Первый список</param>
    /// <param name="b">Второй список</param>
    /// <returns>Возваращает true, если списки одинаковы, false, если нет</returns>
    public async Task<bool> UserDTOListsIsEqual(List<UserDTO> a, List<UserDTO> b)
    {
        for (int i = 0; i < b.Count; i++)
        {
            if (!(a[i].Id == b[i].Id &&
                  a[i].Email == b[i].Email &&
                  a[i].FullName == b[i].FullName &&
                  DateTime.Compare(a[i].RegisteredAt, b[i].RegisteredAt) == 0 &&
                  a[i].Role == b[i].Role)) return false;
        }

        return true;
    }
}