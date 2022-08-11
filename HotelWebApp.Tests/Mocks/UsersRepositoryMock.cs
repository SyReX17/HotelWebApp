using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Repositories;
using HotelWebApp.Sorting;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace HotelWebApp.Tests.Mocks;

public class UsersRepositoryMock : IUserRepository
{
    private List<User> _users;

    public UsersRepositoryMock()
    {
        _users = new List<User>
        {
            new User { Id = 1, Email = "admin@mail.ru", Password = "12345", FullName = "Администратор", RegisteredAt = DateTime.Today, Role = (byte)Role.Admin },
            new User { Id = 2, Email = "user1@mail.ru", Password = "23456", FullName = "Пользователь1", RegisteredAt = DateTime.Today.AddHours(1), Role = (byte)Role.Admin },
            new User { Id = 3, Email = "user2@mail.ru", Password = "55555", FullName = "Пользователь2", RegisteredAt = DateTime.Today.AddHours(2), Role = (byte)Role.Admin  }
        };
    }

    public async Task Add(RegisterData registerData)
    {
        if (_users.FirstOrDefault(u => u.Email == registerData.Email) == null)
        {
            User user = new User 
            { 
                Id = _users.Count(),
                FullName = registerData.FullName, 
                Email = registerData.Email, 
                Password = BC.HashPassword(registerData.Password), 
                RegisteredAt = DateTime.Today,
                Role = Role.User 
            };
            _users.Add(user);
        }
        else
        {
            throw new UserExistsException();
        }
    }

    public async Task<List<User>> GetAll(UserFilter filter)
    {
        List<User> users;

        if (!String.IsNullOrEmpty(filter.FullName) && filter.Date.HasValue)
        {
            users = _users.Where(u =>
                u.FullName.ToLower().Contains(filter.FullName.ToLower()) && 
                DateTime.Compare(u.RegisteredAt, filter.Date.GetValueOrDefault()) == 0).ToList();
        }
        else if (!String.IsNullOrEmpty(filter.FullName))
        {
            users = _users.Where(u => u.FullName.ToLower().Contains(filter.FullName.ToLower())).ToList();
        }
        else if (filter.Date.HasValue)
        {
            users = _users.Where(u => DateTime.Compare(u.RegisteredAt, filter.Date.GetValueOrDefault()) == 0).ToList();
        }
        else
        {
            users = _users;
        }

        if (filter.SortBy.HasValue)
        {
            ISorter<User> sorter = new UserSorter();
            return sorter.Sort(users, (byte)filter.SortBy, filter.SortOrder).ToList();
        }

        return users;
    }

    public async Task<User?> GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }

    public async Task<User?> Get(LoginData loginData)
    {
        return _users.FirstOrDefault(u => u.Email == loginData.Email && BC.Verify(loginData.Password, u.Password));
    }
}