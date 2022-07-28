using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using HotelWebApp.Exceptions;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace HotelWebApp.Controllers
{
    /// <summary>
    /// Контролер аутентификации, включает набор конечных
    /// точек для подключения, отключения и регистрации
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Реализация репозитория для работы с БД
        /// через интерфейс <c>IUserRepository</c>
        /// </summary>
        private IUserRepository _usersRepository;

        /// <summary>
        /// Конструктор контроллера, устанавливает класс,
        /// реализующий интерфейс репозитория
        /// </summary>
        public AuthenticationController()
        {
            this._usersRepository = new UsersRepository();
        }
        
        /// <summary>
        /// Конечная точка для отказа в доступе к ресурсу,
        /// генерирует исключение
        /// </summary>
        /// <exception cref="AccessException"></exception>
        [HttpGet("accessdenied")]
        public async Task Deny()
        {
            throw new AccessException("Доступ отсутствует");
        }
        
        /// <summary>
        /// Конечная точка для логина пользователя,
        /// принимает данные пользователя, возвращает
        /// статусный код или исключение
        /// </summary>
        /// <param name="loginData">
        /// Email и пароль пользователя, полученные из тела запроса
        /// </param>
        /// <returns>
        /// Статусный код Ok(200), если пользователь
        /// успешно добавлен, или исключение <c>AuthenticationException</c>.
        /// </returns>
        /// <exception cref="AuthenticationException"></exception>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginData loginData)
        {
            var user = await _usersRepository.Get(loginData);

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
        
        /// <summary>
        /// Конечная точка для добавления нового пользователя,
        /// принимает данные пользователя, возвращает
        /// статусный код
        /// </summary>
        /// <param name="loginData">
        /// Email и пароль пользователя, полученные из тела запроса
        /// </param>
        /// <returns>
        /// Статусный код Ok(200), если пользователь
        /// успешно добавлен
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginData loginData)
        {
            await _usersRepository.Add(loginData);
            return Ok();
        }
        
        /// <summary>
        /// Конечная точка для выхода из
        /// учетной записи пользователя
        /// возвращает статусный код
        /// </summary>
        /// <returns>
        /// Статусный код Ok(200)
        /// </returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
