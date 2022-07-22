using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using HotelWebApp.Exceptions;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HotelWebApp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("accessdenied")]
        public async Task Deny()
        {
            HttpContext.Response.StatusCode = 403;
            await HttpContext.Response.WriteAsync("Access Denied");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginData loginData)
        {
            Console.WriteLine(loginData.Email);
            Console.WriteLine(loginData.Password);
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Данные введены неверно");
            }
            var user = await UsersRepository.GetUserAsync(loginData.Email, loginData.Password);

            if (user == null) throw new AuthenticationException("Пользователь не найден");

            var role = (Role)user.Role;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            return Ok();
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginData loginData)
        {
            Console.WriteLine(loginData.Email);
            Console.WriteLine(loginData.Password);
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Данные введены неверно");
            }

            await UsersRepository.AddUserAsync(loginData.Email, loginData.Password);
            return Ok();
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
