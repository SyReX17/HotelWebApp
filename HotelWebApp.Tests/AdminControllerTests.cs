using HotelWebApp.Controllers;
using HotelWebApp.Enums;
using HotelWebApp.Filters;
using HotelWebApp.Repositories;
using Moq;

namespace HotelWebApp.Tests;

public class AdminControllerTests
{
    private Mock<IUserRepository> _mockRepo;

    private List<User> _users = new List<User>
    {
        new User
        {
            Email = "admin@mail.ru",
            Password = "12345",
            FullName = "Администратор",
            RegisteredAt = DateTime.Today,
            Role = (byte)Role.Admin
        },
        new User
        {
            Email = "user1@mail.ru",
            Password = "12345",
            FullName = "Пользователь1",
            RegisteredAt = DateTime.Today.AddHours(1),
            Role = (byte)Role.Admin
        },
        new User
        {
            Email = "user2@mail.ru",
            Password = "12345",
            FullName = "Пользователь2",
            RegisteredAt = DateTime.Today.AddHours(2),
            Role = (byte)Role.Admin
        },
    };
    
    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IUserRepository>();
    }

    [Test]
    public async Task GetUsersTest()
    {
        var filter = new UserFilter();
        //_mockRepo.Setup(repo => repo.GetAll(filter)).ReturnsAsync();

        var controller = new AdminUsersController(_mockRepo.Object);

        var result = await controller.GetUsers(filter);
        
        
        
        Assert.Pass();
    }

    public List<User> GetAll()
    {
        return _users;
    }
}