using HotelWebApp.Repositories;

namespace HotelWebApp.Workers;

public class MethodCallService : IMethodCallService
{
    private readonly IBookingRepository _repository;

    public MethodCallService(IBookingRepository bookingRepository)
    {
        _repository = bookingRepository;
    }

    public async Task DoWork()
    {
        await _repository.CheckBookingEnding();
        await _repository.CheckBookingConfirm();
    }
}