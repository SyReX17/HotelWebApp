using HotelWebApp.Enums;
using HotelWebApp.Filters;
using HotelWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWebApp.Repositories;

/// <summary>
/// Класс репозитория для взаимодействия с БД,
/// реализующий интерфейс <c>IBookingRepostory</c>
/// </summary>
public class BookingRepository : IBookingRepository
{
    /// <summary>
    /// Контекст подключения к БД
    /// </summary>
    private ApplicationContext _db;

    public BookingRepository(ApplicationContext context)
    {
        _db = context;
    }
    
    /// <inheritdoc cref="IBookingRepository.Add(Booking booking)"/>
    public async Task Add(Booking booking)
    {
        await _db.Bookings.AddAsync(booking);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc cref="IBookingRepository.UpdateStatus(StatusData statusData)"/>
    public async Task UpdateStatus(StatusData statusData)
    {
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == statusData.BookingId);
        booking.Status = statusData.NewStatus;
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc cref="IBookingRepository.GetAll()"/>
    public async Task<List<Booking>> GetAll()
    {
        return await _db.Bookings.ToListAsync();
    }

    /// <inheritdoc cref="IBookingRepository.GetFreeRooms(BookingFilter filter)"/>
    public async Task<List<HotelRoom>> GetFreeRooms(BookingFilter filter)
    {
        return await _db.Rooms.Include(r => r.Type).Where(r =>  !(_db.Bookings
            .Where(b => (filter.StartAt >= b.StartAt && filter.StartAt <= b.FinishAt) || (filter.FinishAt >= b.StartAt && 
                filter.FinishAt <= b.FinishAt) || (filter.StartAt <= b.StartAt && filter.FinishAt >= b.FinishAt))
            .Select(b => b.RoomId).ToList().Contains(r.Id)) && r.Type.Id == (int)filter.Type).ToListAsync();
    }

    /// <inheritdoc cref="IBookingRepository.CheckBookingEnding()"/>
    public async Task CheckBookingEnding()
    {
        var completedBooking = await _db.Bookings.Where(b => b.FinishAt <= DateTime.Now).ToListAsync();

        if (completedBooking.Count != 0)
        {
            var invoices = new List<Invoice>();

            foreach (var item in completedBooking)
            {
                var invoice = new Invoice
                {
                    Status = InvoiceStatus.Awaiting,
                    BookingId = item.Id,
                    ResidentId = item.ResidentId,
                    ResidentName = await _db.Users.Where(u => u.Id == item.ResidentId).Select(u => u.FullName).FirstOrDefaultAsync(),
                    RoomId = item.RoomId,
                    Price = await GetPrice(item)
                };
                invoices.Add(invoice);
            }

            await _db.Invoices.AddRangeAsync(invoices);
            _db.RemoveRange(completedBooking);
            await _db.SaveChangesAsync();
        }
    }

    /// <inheritdoc cref="IBookingRepository.GetInvoices()"/>
    public async Task<List<Invoice>> GetInvoices()
    {
        return await _db.Invoices.Where(i => i.Status == InvoiceStatus.Awaiting).ToListAsync();
    }

    /// <inheritdoc cref="IBookingRepository.ConfirmInvoice(int invoiceId)"/>
    public async Task ConfirmInvoice(int invoiceId)
    {
        var invoice = await _db.Invoices.Where(i => i.Id == invoiceId).FirstOrDefaultAsync();

        if (invoice != null)
        {
            invoice.Status = InvoiceStatus.Confirm;
            await _db.SaveChangesAsync();
        }
    }

    /// <inheritdoc cref="IBookingRepository.GetPrice(Booking booking)"/>
    public async Task<decimal> GetPrice(Booking booking)
    {
        var basePrice = await _db.Rooms.Include(r => r.Type).Where(r => r.Id == booking.RoomId)
            .Select(r => r.Type.Price).FirstOrDefaultAsync();

        var diffTime = booking.FinishAt.Value.Subtract(booking.StartAt.Value);

        return Convert.ToDecimal(diffTime.TotalMinutes) * (basePrice / 60);
    }

    /// <inheritdoc cref="IBookingRepository.ExtendBooking(int userId, int bookingId, DateTime newFinishAt)"/>
    public async Task ExtendBooking(int userId, int bookingId, DateTime newFinishAt)
    {
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.ResidentId == userId && b.Id == bookingId);

        if (booking != null)
        {
            booking.FinishAt = newFinishAt;

            await _db.SaveChangesAsync();
        }
    }

    /// <inheritdoc cref="IBookingRepository.RemoveBooking(int userId, int bookingId)"/>
    public async Task RemoveBooking(int userId, int bookingId)
    {
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.ResidentId == userId && b.Id == bookingId);

        if (booking != null)
        {
            if (booking.StartAt > DateTime.Now)
            {
                _db.Bookings.Remove(booking);
            }
            else
            {
                booking.FinishAt = DateTime.Today.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
            }
            
            await _db.SaveChangesAsync();
        }
    }

    /// <inheritdoc cref="IBookingRepository.CheckBookingConfirm()"/>
    public async Task CheckBookingConfirm()
    {
        var booking = await _db.Bookings.Where(b => b.StartAt <= DateTime.Now && b.Status != BookingStatus.Confirm).ToListAsync();

        if (booking.Count == 0) return;

        _db.Bookings.RemoveRange(booking);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc cref="IBookingRepository.EvictClient(int userId)"/>
    public async Task EvictClient(int bookingId)
    {
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId && b.StartAt <= DateTime.Now && b.FinishAt >= DateTime.Now);
        
        if (booking != null)
        {
            booking.FinishAt = DateTime.Today.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
            await _db.SaveChangesAsync();
        }
    }
}