using HotelWebApp.Repositories;

namespace HotelWebApp.Workers;

public class BookingWorker : BackgroundService
{
    private IBookingRepository _repository;
    
    private System.Timers.Timer _timer; 
    
    public BookingWorker(IBookingRepository bookingRepository)
    {
        this._repository = bookingRepository;
        this._timer = new System.Timers.Timer();
        this._timer.AutoReset = true;
        this._timer.Interval = 10000;
        this._timer.Elapsed += DoWork;
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._timer.Enabled = true;
    }

    private async void DoWork(object? sender, System.Timers.ElapsedEventArgs e)
    {
        await _repository.CheckBookingEnding();
        await _repository.CheckBookingConfirm();
    }
}