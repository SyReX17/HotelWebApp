using HotelWebApp.Repositories;

namespace HotelWebApp.Workers;

/// <summary>
/// Класс реализюющий интерфейс <c>IMethodCallService</c>
/// </summary>
public class MethodCallService : IMethodCallService
{
    /// <summary>
    /// Реализация репозитория для работы с бронями
    /// </summary>
    private readonly IBookingRepository _repository;

    /// <summary>
    /// Конструктор класса, устанавливающий класс,
    /// реализующий интерфейс репозитория
    /// </summary>
    /// <param name="bookingRepository"></param>
    public MethodCallService(IBookingRepository bookingRepository)
    {
        _repository = bookingRepository;
    }

    /// <inheritdoc cref="IMethodCallService.DoWork()"/>
    public async Task DoWork()
    {
        await _repository.CheckBookingEnding();
        await _repository.CheckBookingConfirm();
    }
}