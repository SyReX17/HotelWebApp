using Microsoft.AspNetCore.Authentication.Cookies;
using HotelWebApp;
using HotelWebApp.MIddlewares;
using HotelWebApp.Repositories;
using HotelWebApp.Services;

var builder = WebApplication.CreateBuilder();
builder.Services.AddSingleton<ApplicationContext>();
builder.Services.AddSingleton<IUserRepository, UsersRepository>();
builder.Services.AddSingleton<IRoomRepository, RoomsRepository>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();
builder.Services.AddHostedService<BookingWorker>();
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
