using HotelWebApp.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelWebApp.Repositories;

/// <summary>
/// Класс репозитория для взаимодействия с БД,
/// реализующий интерфейс <c>IBookingRepostory</c>
/// </summary>
public class BookingsRepository : IBookingsRepository
{
    /// <summary>
    /// Контекст подключения к БД
    /// </summary>
    private ApplicationContext _db;

    /// <summary>
    /// Конструктор, принимает контекст подключения к БД
    /// </summary>
    /// <param name="context">Контекст подключения к БД</param>
    public BookingsRepository(ApplicationContext context)
    {
        _db = context;
    }
    
    /// <inheritdoc cref="IBookingsRepository.Add(Booking booking)"/>
    public async Task Add(Booking booking)
    {
        await _db.Bookings.AddAsync(booking);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc cref="IBookingsRepository.GetAll()"/>
    public async Task<List<Booking>> GetAll()
    {
        return await _db.Bookings.ToListAsync();
    }

    /// <inheritdoc cref="IBookingsRepository.GetComletedBookings()"/>
    public async Task<List<Booking>> GetComletedBookings()
    {
        return await _db.Bookings.Where(b => b.FinishAt <= DateTime.Now).ToListAsync();
    }

    /// <inheritdoc cref="IBookingsRepository.GetUnconfirmedBookings()"/>
    public async Task<List<Booking>> GetUnconfirmedBookings()
    {
        return await _db.Bookings.Where(b => b.StartAt <= DateTime.Now && b.Status != BookingStatus.Confirm).ToListAsync();
    }

    /// <inheritdoc cref="IBookingsRepository.Remove(Booking booking)"/>
    public async Task Remove(Booking booking)
    {
        _db.Bookings.Remove(booking);
    }

    /// <inheritdoc cref="IBookingsRepository.GetById(int bookingId)"/>
    public async Task<Booking?> GetById(int bookingId)
    {
        return await _db.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    /// <inheritdoc cref="IBookingsRepository.RemoveRange()"/>
    public async Task RemoveRange(List<Booking> list)
    {
        _db.Bookings.RemoveRange(list);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc cref="IBookingsRepository.Update(Booking booking)"/>
    public async Task Update(Booking booking)
    {
        _db.Bookings.Update(booking);
        await _db.SaveChangesAsync();
    }
}