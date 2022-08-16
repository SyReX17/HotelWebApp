﻿using System.Security.Authentication;
using HotelWebApp.Exceptions;
using HotelWebApp.MIddlewares;
using HotelWebApp.Repositories;
using HotelWebApp.Workers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HotelWebApp;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>();
        
        services.AddScoped<IUserRepository, UsersRepository>();
        services.AddScoped<IRoomRepository, RoomsRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        
        services.AddScoped<IMethodCallService, MethodCallService>();
        services.AddHostedService<TimerBackgroundService>();
        
        services.AddControllers();
        
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                throw new UnauthorizedException();
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                throw new AccessDeniedException();
            };
        });
        services.AddAuthorization();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();

        if (env.IsDevelopment())
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}