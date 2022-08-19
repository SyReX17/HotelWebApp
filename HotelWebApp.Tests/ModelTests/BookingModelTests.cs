using HotelWebApp.Enums;

namespace HotelWebApp.Tests.ModelTests;

/// <summary>
/// Класс для тестирования модели брони
/// </summary>
public class BookingModelTests
{
    /// <summary>
    /// Метод для тестирования подтверждения брони
    /// </summary>
    [Test]
    public async Task ConfirmTests()
    {
        var booking = new Booking
        {
            Id = 1,
            ResidentId = 2,
            RoomId = 1,
            Status = BookingStatus.Awaiting,
            StartAt = DateTime.Now.AddMinutes(5),
            FinishAt = DateTime.Now.AddMinutes(10),
        };
        
        booking.Confirm();
        
        Assert.AreEqual(booking.Status, BookingStatus.Confirm);
    }

    /// <summary>
    /// Метод для тестирования прекращения проживания
    /// </summary>
    [Test]
    public async Task StopTests()
    {
        var booking = new Booking
        {
            Id = 1,
            ResidentId = 2,
            RoomId = 1,
            Status = BookingStatus.Awaiting,
            StartAt = DateTime.Now.AddHours(5),
            FinishAt = DateTime.Now.AddHours(10),
        };
        
        booking.Stop();
        
        Assert.IsTrue(booking.FinishAt <= DateTime.Now);
    }

    /// <summary>
    /// Метод для тестирования вычисления стоимости проживания
    /// </summary>
    [Test]
    public async Task GetPriceTests()
    {
        var booking = new Booking
        {
            Id = 1,
            ResidentId = 2,
            RoomId = 1,
            Status = BookingStatus.Awaiting,
            StartAt = DateTime.Now.AddHours(0),
            FinishAt = DateTime.Now.AddHours(1),
        };

        var price = booking.GetPrice(60);

        Console.WriteLine(price);
        
        Assert.IsTrue(price - Convert.ToDecimal(60) <= Convert.ToDecimal(Math.Pow(10, -6)));
    }
}