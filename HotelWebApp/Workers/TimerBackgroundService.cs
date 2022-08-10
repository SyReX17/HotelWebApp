namespace HotelWebApp.Workers;

public class TimerBackgroundService : BackgroundService
{
    private readonly System.Timers.Timer _timer;

    private IServiceProvider Services;

    public TimerBackgroundService(IServiceProvider services)
    {
        Services = services;
        _timer = new System.Timers.Timer();
        _timer.AutoReset = true;
        _timer.Interval = 10000;
        _timer.Elapsed += DoWork;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer.Enabled = true;
    }

    public async void DoWork(object? sender, System.Timers.ElapsedEventArgs e)
    {
        using (var scope = Services.CreateScope())
        {
            var scopeService = scope.ServiceProvider.GetRequiredService<IMethodCallService>();

            await scopeService.DoWork();
        }
    }
}