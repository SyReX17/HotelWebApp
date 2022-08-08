using HotelWebApp.Repositories;
using System.Timers;
using HotelWebApp.Models;
using Timer = System.Timers.Timer;

namespace HotelWebApp;

public class RepositoryTimer
{
    
    private Timer _timer; 
    
    private IBookingRepository _repository;

    public RepositoryTimer(int interval)
    {
        this._timer = new Timer();
        this._timer.AutoReset = true;
        this._timer.Interval = interval;
        this._timer.Elapsed += MethodCall;
        this._timer.Enabled = true;
    }

    private async void MethodCall(object sender, System.Timers.ElapsedEventArgs e)
    {
        await _repository.CheckBookingEnding();
        await _repository.CheckBookingConfirm();
    }
}