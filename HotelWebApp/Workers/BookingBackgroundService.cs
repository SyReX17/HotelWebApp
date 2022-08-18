using HotelWebApp.Interfaces.Services;

namespace HotelWebApp.Workers;

/// <summary>
/// Класс для фоновой задачи таймера
/// </summary>
public class BookingBackgroundService : BackgroundService
{
    /// <summary>
    /// Таймер
    /// </summary>
    private readonly System.Timers.Timer _timer;

    /// <summary>
    /// Провайдер сервисов
    /// </summary>
    private IServiceProvider Services;

    /// <summary>
    /// Конструктор класса, инициализирующий таймер
    /// </summary>
    /// <param name="services"></param>
    public BookingBackgroundService(IServiceProvider services)
    {
        Services = services;
        _timer = new System.Timers.Timer(10000);
        _timer.AutoReset = true;
        _timer.Elapsed += DoWork;
    }

    /// <summary>
    /// Переопределение метода родительского класса <c>BackgroundService</c>,
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer.Enabled = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Метод, вызываемый таймером с определенным интервалом времени,
    /// создает scope сервис для вызова методов из другого класса
    /// </summary>
    public async void DoWork(object? sender, System.Timers.ElapsedEventArgs e)
    {
        using (var scope = Services.CreateScope())
        {
            var scopeService = scope.ServiceProvider.GetRequiredService<IBookingsService>();

            await scopeService.CheckBookingConfirm();
            await scopeService.CheckBookingEnding();
        }
    }
}