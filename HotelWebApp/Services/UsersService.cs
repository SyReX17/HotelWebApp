using System.Security.Claims;
using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Interfaces.Services;
using HotelWebApp.Models;
using HotelWebApp.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace HotelWebApp.Services;

public class UsersService : IUsersService
{
    /// <summary>
    /// Интерфейс репозитория для работы с пользователями
    /// </summary>
    private readonly IUsersRepository _repository;

    public UsersService(IUsersRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc cref="IUsersService.GetAll(UserFilter filter)"/>
    public async Task<List<UserDTO>> GetAll(UserFilter filter)
    {
        var users =  await _repository.GetAll(filter);

        return users.Select(u => u.ToUserDTO()).ToList();
    }

    /// <inheritdoc cref="IUsersService.GetUserClaims(LoginData loginData)"/>
    public async Task<List<Claim>> GetUserClaims(LoginData loginData)
    {
        var user = await GetByEmail(loginData.Email);

        if (!BC.Verify(loginData.Password, user.Password)) throw new PasswordValidationException();
            
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
        };

        return claims;
    }

    /// <inheritdoc cref="IUsersService.Add(RegisterData registerData)"/>
    public async Task Add(RegisterData registerData)
    {
        var user = await _repository.GetByEmail(registerData.Email);
            
        if (user != null) throw new UserExistsException();
            
        registerData.Password = BC.HashPassword(registerData.Password);
        
        User newUser = new User 
        { 
            FullName = registerData.FullName, 
            Email = registerData.Email, 
            Password = BC.HashPassword(registerData.Password), 
            RegisteredAt = DateTime.Today,
            Role = Role.User 
        };
            
        await _repository.Add(newUser);
    }

    /// <inheritdoc cref="IUsersService.GetByEmail(string email)"/>
    public async Task<User> GetByEmail(string email)
    {
        var user = await _repository.GetByEmail(email);

        if (user == null) throw new UserNotFoundException();

        return user;
    }
}