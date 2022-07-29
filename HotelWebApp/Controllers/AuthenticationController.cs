using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using HotelWebApp.Exceptions;
using HotelWebApp.Repositories;
using HotelWebApp.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

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
        /// <response code="403">Отсутсвует доступ к запрашиваемому ресурсу</response>
        /// <exception cref="AccessException"></exception>
        [ProducesResponseType(403)]
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
        /// <response code="200">Успешная аутентификация пользователя</response>
        /// <response code="400">Данные введены неверно</response>
        /// <response code="401">Пользователь не найден</response>
        /// <exception cref="AuthenticationException">Пользователь не найден</exception>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody]LoginData loginData)
        {
            var user = await _usersRepository.Get(loginData);

            if (user == null) throw new AuthenticationException("Пользователь не найден");

            var role = user.Role;
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
        /// <response code="200">Успешная регистрация пользователя</response>
        /// <response code="400">Данные введены некоректно</response>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterData registerData)
        {
            try
            {
                await _usersRepository.Add(registerData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserExistsException("Пользователь уже существует");
            }
            
            return Ok();
        }
        
        /// <summary>
        /// Конечная точка для выхода из
        /// учетной записи пользователя
        /// возвращает статусный код
        /// </summary>
        /// <response code="200">Успешный выход пользователя из учетной записи</response>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
