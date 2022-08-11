using System.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using HotelWebApp.Exceptions;
using HotelWebApp.Repositories;
using Microsoft.AspNetCore.Identity;
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
        /// Реализация репозитория для работы с БД
        /// через интерфейс <c>IUserRepository</c>
        /// </summary>
        private readonly IUserRepository _usersRepository;

        /// <summary>
        /// Конструктор контроллера, устанавливает класс,
        /// реализующий интерфейс репозитория
        /// </summary>
        public AuthenticationController(IUserRepository userRepository)
        {
            _usersRepository = userRepository;
        }

        private PasswordHasher<User> _hasher = new PasswordHasher<User>();
          
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
            throw new AccessDeniedException(  );
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
            var user = await _usersRepository.GetByEmail(loginData.Email);
            
            if (user == null) throw new UserNotFoundException();

            if (!BC.Verify(loginData.Password, user.Password)) throw new PasswordValidationException();

            var role = user.Role;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
            };
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
        /// <response code="200">Успешная регистрация пользователя</response>
        /// <response code="400">Данные введены некоректно</response>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterData registerData)
        {
            registerData.Password = BC.HashPassword(registerData.Password);
            await _usersRepository.Add(registerData);
            
            return NoContent();
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
            return NoContent();
        }
    }
}
