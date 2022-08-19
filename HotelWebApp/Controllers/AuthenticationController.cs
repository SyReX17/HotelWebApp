using System.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using HotelWebApp.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace HotelWebApp.Controllers
{
    /// <summary>
    /// Контролер аутентификации, включает набор конечных
    /// точек для подключения, отключения и регистрации
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Интерфейс сервиса для работы с пользователями
        /// </summary>
        private readonly IUsersService _usersService;

        /// <summary>
        /// Конструктор контроллера, устанавливает класс,
        /// реализующий интерфейс сервиса
        /// </summary>
        /// <param name="usersService">Сервис для работы с пользователями</param>
        public AuthenticationController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        
        /// <summary>
        /// Конечная точка для логина пользователя,
        /// принимает данные пользователя, возвращает
        /// статусный код или исключение
        /// </summary>
        /// <param name="loginData">
        /// Email и пароль пользователя, полученные из тела запроса
        /// </param>
        /// <response code="204">Успешная аутентификация пользователя</response>
        /// <response code="400">Данные введены неверно</response>
        /// <response code="401">Пользователь не найден</response>
        /// <exception cref="AuthenticationException">Пользователь не найден</exception>
        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody]LoginData loginData)
        {
            var claims = await _usersService.GetUserClaims(loginData);
            
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            
            await HttpContext.SignInAsync(claimsPrincipal);
            
            return NoContent();
        }
        
        /// <summary>
        /// Конечная точка для добавления нового пользователя,
        /// принимает данные пользователя, возвращает
        /// статусный код
        /// </summary>
        /// <param name="loginData">
        /// Email и пароль пользователя, полученные из тела запроса
        /// </param>
        /// <response code="204">Успешная регистрация пользователя</response>
        /// <response code="400">Данные введены некоректно</response>
        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterData registerData)
        {
            await _usersService.Add(registerData);
            
            return NoContent();
        }
        
        /// <summary>
        /// Конечная точка для выхода из
        /// учетной записи пользователя
        /// возвращает статусный код
        /// </summary>
        /// <response code="204">Успешный выход пользователя из учетной записи</response>
        [HttpPost("logout")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return NoContent();
        }
    }
}
