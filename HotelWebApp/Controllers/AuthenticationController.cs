using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using HotelWebApp.Exceptions;
using HotelWebApp.Repositories;

namespace HotelWebApp.Controllers
{
    public static class AuthenticationController
    {
        public static async Task Deny(HttpContext context)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access Denied");
        }

        public static async Task<IResult> Login(LoginData loginData, HttpContext context)
        {
            if (loginData.Email == "" || loginData.Password == "")
            {
                throw new RequestException("Email и/или пароль не установлены");
            }

            User? user = await UsersRepository.GetUserAsync(loginData.Email, loginData.Password);

            if (user == null) throw new AuthenticationException("Пользователь не найден");

            var role = (Role)user.Role;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await context.SignInAsync(claimsPrincipal);
            return Results.Ok();
        }

        public static async Task<IResult> Register(LoginData loginData, HttpContext context)
        {
            if (loginData.Email == "" || loginData.Password == "")
            {
                throw new RequestException("Email и/или пароль не установлены");
            }

            await UsersRepository.AddUserAsync(loginData.Email, loginData.Password);
            return Results.Ok();
        }

        public static async Task<IResult> Logout(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Ok();
        }
    }
}
