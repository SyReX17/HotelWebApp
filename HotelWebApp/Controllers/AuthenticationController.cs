using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace HotelWebApp.Controllers
{
    public static class AuthenticationController
    {
        public static async Task Deny(HttpContext context)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access Denied");
        }

        public async static Task<IResult> Login(LoginData loginData, HttpContext context)
        {
            Console.WriteLine(loginData.Email);
            Console.WriteLine(loginData.Password);
            if (loginData.Email == "" || loginData.Password == "")
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Email и/или пароль не установлены");
            }

            User? user = await UsersRepository.GetUserAsync(loginData.Email, loginData.Password);

            if (user == null) return Results.Unauthorized();

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

        public async static Task<IResult> Register(LoginData loginData, HttpContext context)
        {
            if (loginData.Email == "" || loginData.Password == "")
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Email и/или пароль не установлены");
            }

            await UsersRepository.AddUserAsync(loginData.Email, loginData.Password);
            return Results.Ok();
        }

        public async static Task<IResult> Logout(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Ok();
        }
    }
}
