using HotelWebApp.Repositories;

namespace HotelWebApp.Workers;

public class BookingWorker : BackgroundService
{
    private readonly IBookingRepository _repository;
    
    private readonly System.Timers.Timer _timer; 
    
    public BookingWorker(IBookingRepository bookingRepository)
    {
        _repository = bookingRepository;
        _timer = new System.Timers.Timer();
        _timer.AutoReset = true;
        _timer.Interval = 10000;
        _timer.Elapsed += DoWork;
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer.Enabled = true;
    }

    private async void DoWork(object? sender, System.Timers.ElapsedEventArgs e)
    {
        await _repository.CheckBookingEnding();
        await _repository.CheckBookingConfirm();
    }
}