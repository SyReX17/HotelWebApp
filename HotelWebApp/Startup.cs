using HotelWebApp.Exceptions;
using HotelWebApp.Interfaces.Services;
using HotelWebApp.MIddlewares;
using HotelWebApp.Repositories;
using HotelWebApp.Services;
using HotelWebApp.Workers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace HotelWebApp;

public class Startup
{
    public IConfiguration Configuration;
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IRoomsRepository, RoomsRepository>();
        services.AddScoped<IBookingsRepository, BookingsRepository>();
        services.AddScoped<IInvoicesRepository, InvoicesRepository>();

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IRoomsService, RoomsService>();
        services.AddScoped<IBookingsService, BookingsService>();
        services.AddScoped<IInvoicesService, InvoicesService>();

        services.AddHostedService<BookingBackgroundService>();
        
        services.AddControllers();
        
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Events.OnRedirectToLogin = context => throw new UnauthorizedException();
            options.Events.OnRedirectToAccessDenied = context => throw new AccessDeniedException();
        });
        services.AddAuthorization();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
        
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var scopeProvider = scope.ServiceProvider;
            var context = scopeProvider.GetRequiredService<ApplicationContext>();
            await ProjectInitializer.Initialize(context);
        }
    }
}