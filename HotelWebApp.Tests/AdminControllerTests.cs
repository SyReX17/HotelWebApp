using HotelWebApp.Controllers;
using HotelWebApp.Enums;
using HotelWebApp.Filters;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using HotelWebApp.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelWebApp.Tests;

public class AdminControllerTests
{
    private IUserRepository _mockRepo;

    private List<UserDTO> _testList;
    [SetUp]
    public void Setup()
    {
        _mockRepo = new UsersRepositoryMock();
        _testList = new List<UserDTO>
        {
            new UserDTO { Id = 1, Email = "admin@mail.ru", FullName = "Администратор", RegisteredAt = DateTime.Today, Role = (byte)Role.Admin },
            new UserDTO { Id = 2, Email = "user1@mail.ru", FullName = "Пользователь1", RegisteredAt = DateTime.Today.AddHours(1), Role = (byte)Role.Admin },
            new UserDTO { Id = 3, Email = "user2@mail.ru", FullName = "Пользователь2", RegisteredAt = DateTime.Today.AddHours(2), Role = (byte)Role.Admin  }
        };
    }

    [Test]
    public async Task GetAllUsersTest()
    {
        var filter = new UserFilter();

        var controller = new AdminUsersController(_mockRepo);

        var result = await controller.GetUsers(filter) as OkObjectResult;

        var listResult = result.Value as List<UserDTO>;

        foreach (var item in listResult)
        {
            Console.WriteLine(item.Id);
        }

        Assert.NotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsTrue(_testList.SequenceEqual(listResult));
    }
}