using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Repositories;
using HotelWebApp.Services;
using Moq;

namespace HotelWebApp.Tests.ServicesTests;

/// <summary>
/// Класс для тестирования сервиса бронирования
/// </summary>
public class BookingServiceTests
{
    /// <summary>
    /// Тестовый список с данными о бронировании
    /// </summary>
    private List<Booking> _testData;

    /// <summary>
    /// Тестовый пользователь
    /// </summary>
    private User _testUser;
    
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

        _testUser = new User
        {
            Id = 1,
            Email = "admin@mail.ru",
            Password = "12345",
            FullName = "Администратор",
            RegisteredAt = DateTime.Today,
            Role = Role.Admin
        };
    }

    /// <summary>
    /// Метод для тестирования получения всех броней
    /// </summary>
    [Test]
    public async Task GetAllBookingsTests()
    {
        var bookingsRepoMock = new Mock<IBookingsRepository>();
        var roomsRepoMock = new Mock<IRoomsRepository>();
        var usersRepoMock = new Mock<IUsersRepository>();
        var invoicesRepoMock = new Mock<IInvoicesRepository>();
        
        bookingsRepoMock.Setup(repo => repo.GetAll()).ReturnsAsync(_testData);

        var service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);

        var result = await service.GetAll();
        
        Assert.IsNotNull(result);
        Assert.IsTrue(await BookingListIsEqual(result, _testData));
    }

    /// <summary>
    /// Метод для тестирования подтверждения брони
    /// </summary>
    [Test]
    public async Task ConfirmBookingTests()
    {
        var bookingsRepoMock = new Mock<IBookingsRepository>();
        var roomsRepoMock = new Mock<IRoomsRepository>();
        var usersRepoMock = new Mock<IUsersRepository>();
        var invoicesRepoMock = new Mock<IInvoicesRepository>();

        Booking nullResult = null;
        
        bookingsRepoMock.Setup(repo => repo.GetById(1)).ReturnsAsync(nullResult);
        
        var service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);

        try
        {
            var result = await service.GetById(1);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is BookingNotFoundException);
        }
    }

    /// <summary>
    /// Метод для тестирования продления брони
    /// </summary>
    [Test]
    public async Task ExtendBookingTests()
    {
        var bookingsRepoMock = new Mock<IBookingsRepository>();
        var roomsRepoMock = new Mock<IRoomsRepository>();
        var usersRepoMock = new Mock<IUsersRepository>();
        var invoicesRepoMock = new Mock<IInvoicesRepository>();
        
        Booking nullResult = null;
        
        bookingsRepoMock.Setup(repo => repo.GetById(1)).ReturnsAsync(nullResult);
        usersRepoMock.Setup(repo => repo.GetByEmail("admin@mail.ru")).ReturnsAsync(_testUser);
        
        var service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);

        try
        {
            await service.ExtendBooking("admin@mail.ru", 1, DateTime.Now);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is BookingNotFoundException);
        }

        var booking = new Booking()
        {
            Id = 1,
            ResidentId = 2,
            RoomId = 1,
            Status = BookingStatus.Awaiting,
            StartAt = DateTime.Now.AddMinutes(5),
            FinishAt = DateTime.Now.AddMinutes(10)
        };

        bookingsRepoMock.Setup(repo => repo.GetById(1)).ReturnsAsync(booking);
        
        service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);

        try
        {
            await service.ExtendBooking("admin@mail.ru", 1, DateTime.Now);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is BookingNotFoundException);
        }
    }

    /// <summary>
    /// Метод для тестирования отмены брони
    /// </summary>
    [Test]
    public async Task CancelBookingTests()
    {
        var bookingsRepoMock = new Mock<IBookingsRepository>();
        var roomsRepoMock = new Mock<IRoomsRepository>();
        var usersRepoMock = new Mock<IUsersRepository>();
        var invoicesRepoMock = new Mock<IInvoicesRepository>();
        
        Booking nullResult = null;
        
        bookingsRepoMock.Setup(repo => repo.GetById(1)).ReturnsAsync(nullResult);
        
        var service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);

        try
        {
            await service.CancelBooking("admin@mail.ru", 1);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is BookingNotFoundException);
        }
        
        var booking = new Booking()
        {
            Id = 1,
            ResidentId = 2,
            RoomId = 1,
            Status = BookingStatus.Awaiting,
            StartAt = DateTime.Now.AddMinutes(5),
            FinishAt = DateTime.Now.AddMinutes(10)
        };

        bookingsRepoMock.Setup(repo => repo.GetById(1)).ReturnsAsync(booking);
        usersRepoMock.Setup(repo => repo.GetByEmail("admin@mail.ru")).ReturnsAsync(_testUser);
        
        service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);

        try
        {
            await service.CancelBooking("admin@mail.ru", 1);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is BookingNotFoundException);
        }
    }

    /// <summary>
    /// Метод для тестирования потелучения брони по идентификатору
    /// </summary>
    [Test]
    public async Task GetByIdTests()
    {
        var bookingsRepoMock = new Mock<IBookingsRepository>();
        var roomsRepoMock = new Mock<IRoomsRepository>();
        var usersRepoMock = new Mock<IUsersRepository>();
        var invoicesRepoMock = new Mock<IInvoicesRepository>();
        
        Booking nullResult = null;
        
        bookingsRepoMock.Setup(repo => repo.GetById(1)).ReturnsAsync(nullResult);
        
        var service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);
        
        try
        {
            await service.GetById(1);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is BookingNotFoundException);
        }
        
        var booking = new Booking()
        {
            Id = 1,
            ResidentId = 2,
            RoomId = 1,
            Status = BookingStatus.Awaiting,
            StartAt = DateTime.Now.AddMinutes(5),
            FinishAt = DateTime.Now.AddMinutes(10)
        };

        bookingsRepoMock.Setup(repo => repo.GetById(1)).ReturnsAsync(booking);
        
        service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);

        var result = await service.GetById(1);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(result, booking);
    }

    /// <summary>
    /// Метод для тестирования выселения клиента
    /// </summary>
    [Test]
    public async Task EvictClientTests()
    {
        var bookingsRepoMock = new Mock<IBookingsRepository>();
        var roomsRepoMock = new Mock<IRoomsRepository>();
        var usersRepoMock = new Mock<IUsersRepository>();
        var invoicesRepoMock = new Mock<IInvoicesRepository>();
        
        Booking nullResult = null;
        
        bookingsRepoMock.Setup(repo => repo.GetById(1)).ReturnsAsync(nullResult);
        
        var service = new BookingsService(bookingsRepoMock.Object, usersRepoMock.Object, roomsRepoMock.Object, invoicesRepoMock.Object);

        try
        {
            await service.EvictClient(1);
        }
        catch (RequestException e)
        {
            Assert.IsTrue(e is BookingNotFoundException);
        }
    }
    
    /// <summary>
    /// Метод для проверки эквивалентности значений списка
    /// </summary>
    /// <param name="a">Первый список</param>
    /// <param name="b">Второй список</param>
    /// <returns>Возваращает true, если списки одинаковы, false, если нет</returns>
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