using Microsoft.AspNetCore.Authentication.Cookies;
using HotelWebApp;
using HotelWebApp.MIddlewares;
using HotelWebApp.Repositories;
using HotelWebApp.Workers;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IRoomRepository, RoomsRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IMethodCallService, MethodCallService>();
builder.Services.AddHostedService<TimerBackgroundService>();

builder.Services.AddControllers();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });

    IInitializer initializer = new ProjectInitializer();
    var context = new ApplicationContext();
    initializer.Initialize(context);
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
