using System.Security.Claims;
using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using HotelWebApp.Services;
using Moq;

namespace HotelWebApp.Tests.ServicesTests;

/// <summary>
/// Класс для тестирования сервиса для работы с пользователями
/// </summary>
public class UsersServiceTests
{
    /// <summary>
    /// Тестовые данные
    /// </summary>
    private List<User> _testUsers;
    
    /// <summary>
    /// Метод для установки начальных значений
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _testUsers = new List<User>
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

    [Test]
    public async Task GetAllUsersTests()
    {
        var filter = new UserFilter();
        
        var mock = new Mock<IUsersRepository>();
        
        mock.Setup(repo => repo.GetAll(filter)).ReturnsAsync(_testUsers);
        
        var service = new UsersService(mock.Object);

        var testResultList = new List<UserDTO>();

        foreach (var user in _testUsers)
        {
            testResultList.Add(user.ToUserDTO());
        }

        var result = await service.GetAll(filter);
        
        Assert.IsNotNull(result);

        Assert.IsTrue( await UserDTOListsIsEqual(testResultList, result));
    }

    [Test]
    public async Task GetUserClaimsTests()
    {
        var testClaims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, _testUsers[0].Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, _testUsers[0].Role.ToString())
        };

        var mock = new Mock<IUsersRepository>();

        var testUser = new User
        {
            Id = 1,
            Email = "admin@mail.ru",
            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
            FullName = "Администратор",
            RegisteredAt = DateTime.Today,
            Role = Role.Admin
        };
        mock.Setup(repo => repo.GetByEmail(_testUsers[0].Email)).ReturnsAsync(testUser);

        var service = new UsersService(mock.Object);

        var loginData = new LoginData
        {
            Email = _testUsers[0].Email,
            Password = _testUsers[0].Password
        };

        var result = await service.GetUserClaims(loginData);
        
        Assert.IsNotNull(result);
        Assert.IsTrue(await ClaimListsAreEqual(result, testClaims));

        loginData.Password = "76543";

        try
        {
            await service.GetUserClaims(loginData);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is PasswordValidationException);
        }
    }

    [Test]
    public async Task AddUserTests()
    {
        var registerDataTest = new RegisterData
        {
            Email = "test@mail.ru",
            Password = "55555",
            FullName = "Пользователь"
        };
        var mock = new Mock<IUsersRepository>();
        mock.Setup(repo => repo.GetByEmail("test@mail.ru")).ReturnsAsync(_testUsers[0]);

        var service = new UsersService(mock.Object);

        try
        {
            await service.Add(registerDataTest);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is UserExistsException);
        }
    }

    [Test]
    public async Task GetUserByEmailTests()
    {
        User? nullUser = null;

        var mock = new Mock<IUsersRepository>();
        mock.Setup(repo => repo.GetByEmail("")).ReturnsAsync(nullUser);

        var service = new UsersService(mock.Object);

        try
        {
            await service.GetByEmail("");
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is UserNotFoundException);
        }

        mock.Setup(repo => repo.GetByEmail("")).ReturnsAsync(_testUsers[0]);

        service = new UsersService(mock.Object);

        var result = await service.GetByEmail("");
        
        Assert.IsNotNull(result);
        Assert.AreEqual(_testUsers[0], result);
    }

    public async Task<bool> ClaimListsAreEqual(List<Claim> a, List<Claim> b)
    {
        for (int i = 1; i < b.Count; i++)
        {
            if (a[i].Value != b[i].Value) return false;
        }

        return true;
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