using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Interfaces.Services;
using HotelWebApp.Models;
using HotelWebApp.Repositories;

namespace HotelWebApp.Services;

public class BookingsService : IBookingsService
{
    /// <summary>
    /// Интерфейс репозитория для работы с бронями
    /// </summary>
    private readonly IBookingsRepository _bookingsRepository;

    /// <summary>
    /// Интерфейс репозитория для работы с пользователями
    /// </summary>
    private readonly IUsersRepository _usersRepository;

    /// <summary>
    /// Интерфейс репозитория для работы с комнатыми
    /// </summary>
    private readonly IRoomsRepository _roomsRepository;

    /// <summary>
    /// Интерфейс репозитория для работы с счетами на оплату
    /// </summary>
    private readonly IInvoicesRepository _invoicesRepository;
    
    public BookingsService(
        IBookingsRepository bookingsRepository, IUsersRepository usersRepository,
        IRoomsRepository roomsRepository, IInvoicesRepository invoicesRepository)
    {
        _bookingsRepository = bookingsRepository;
        _usersRepository = usersRepository;
        _roomsRepository = roomsRepository;
        _invoicesRepository = invoicesRepository;
    }
    
    /// <inheritdoc cref="IBookingsService.Add(UserBookingData data, int userId)"/>
    public async Task<Booking> Add(UserBookingData data, int userId)
    {
        if (data.StartAt >= data.FinishAt || data.StartAt < DateTime.Now)
        {
            throw new DatesValidationException();
        }

        var booking = new Booking
        {
            ResidentId = userId,
            RoomId = data.RoomId,
            Status = BookingStatus.Awaiting,
            StartAt = data.StartAt,
            FinishAt = data.FinishAt
        };

        await _bookingsRepository.Add(booking);

        return booking;
    }

    /// <inheritdoc cref="IBookingsService.GetAll()"/>
    public async Task<List<Booking>> GetAll()
    {
        return await _bookingsRepository.GetAll();
    }

    /// <inheritdoc cref="IBookingsService.ExtendBooking(int userId, int bookingId, DateTime newFinishAt)"/>
    public async Task ExtendBooking(int userId, int bookingId, DateTime newFinishAt)
    {
        var booking = await _bookingsRepository.GetById(bookingId);

        if (booking == null) throw new BookingNotFoundException();

        if (booking.ResidentId != userId) throw new BookingNotFoundException();

        booking.FinishAt = newFinishAt;

        await _bookingsRepository.SaveChanges();
    }

    /// <inheritdoc cref="IBookingsService.ConfirmBooking(int bookingId)"/>
    public async Task ConfirmBooking(int bookingId)
    {
        var booking = await _bookingsRepository.GetById(bookingId);
        
        if (booking == null) throw new BookingNotFoundException();

        booking.Confirm();

        await _bookingsRepository.SaveChanges();
    }

    /// <inheritdoc cref="IBookingsService.CancelBooking(int userId, int bookingId)"/>
    public async Task CancelBooking(int userId, int bookingId)
    {
        var booking = await _bookingsRepository.GetById(bookingId);

        if (booking == null) throw new BookingNotFoundException();

        if (booking.ResidentId != userId) throw new BookingNotFoundException();

        if (booking.StartAt > DateTime.Now)
        {
            await _bookingsRepository.Remove(booking);
        }
        else
        {
            booking.Stop();
        }

        await _bookingsRepository.SaveChanges();
    }

    /// <inheritdoc cref="IBookingsService.CheckBookingEnding()"/>
    public async Task CheckBookingEnding()
    {
        var bookings = await _bookingsRepository.GetComletedBookings();
        
        if (bookings.Count != 0)
        {
            var invoices = new List<Invoice>();

            foreach (var item in bookings)
            {
                var room = await _roomsRepository.GetById(item.RoomId);

                var user = await _usersRepository.GetById(item.ResidentId);

                var basePrice = room.Type.Price;
                
                var invoice = new Invoice
                {
                    Status = InvoiceStatus.Awaiting,
                    BookingId = item.Id,
                    ResidentId = item.ResidentId,
                    ResidentName = user.FullName,
                    RoomId = item.RoomId,
                    Price = item.GetPrice(basePrice)
                };
                invoices.Add(invoice);
            }

            await _invoicesRepository.AddRange(invoices);
            await _bookingsRepository.RemoveRange(bookings);
        }
    }

    /// <inheritdoc cref="IBookingsService.CheckBookingConfirm()"/>
    public async Task CheckBookingConfirm()
    {
        var bookings = await _bookingsRepository.GetUnconfirmedBookings();

        if (bookings.Count == 0) return;

        await _bookingsRepository.RemoveRange(bookings);
    }

    /// <inheritdoc cref="IBookingsService.GetById(int bookingId)"/>
    public async Task<Booking> GetById(int bookingId)
    {
        var booking = await _bookingsRepository.GetById(bookingId);

        if (booking == null) throw new BookingNotFoundException();

        return booking;
    }
}