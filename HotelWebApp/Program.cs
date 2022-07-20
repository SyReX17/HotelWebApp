using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using HotelWebApp;
using HotelWebApp.Controllers;

var builder = WebApplication.CreateBuilder();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/accessdenied";
    });
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", async (LoginData loginData, HttpContext context) => await AuthenticationController.Login(loginData, context));
app.MapPost("/register", async (LoginData loginData, HttpContext context) => await AuthenticationController.Register(loginData, context));
app.MapGet("/accessdenied", async (HttpContext context) => await AuthenticationController.Deny(context));
app.MapDelete("/logout", async (HttpContext context) => await AuthenticationController.Logout(context));

app.Run();
