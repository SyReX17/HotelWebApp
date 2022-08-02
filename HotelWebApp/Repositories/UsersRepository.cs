using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Enums;
using HotelWebApp.Exceptions;
using HotelWebApp.Filters;
using HotelWebApp.Sorting;

namespace HotelWebApp.Repositories
{
    /// <summary>
    /// Класс репозитория для взаимодействия с БД,
    /// реализует интерфейс <c>IUserRepository</c>
    /// </summary>
    public class UsersRepository : IUserRepository
    {
        /// <summary>
        /// Контекст подключения к БД
        /// </summary>
        private ApplicationContext _db = new ApplicationContext();

        /// <inheritdoc cref="IUserRepository.GetAll(UserFilter filter)"/>
        public async Task<List<User>> GetAll(UserFilter filter)
        {
            List<User> users;

            if (!String.IsNullOrEmpty(filter.FullName) && filter.Date.HasValue)
            {
                users = await _db.Users.Where(u =>
                    u.FullName.ToLower().Contains(filter.FullName.ToLower()) && 
                    DateTime.Compare(u.RegisteredAt, filter.Date.GetValueOrDefault()) == 0).ToListAsync();
            }
            else if (!String.IsNullOrEmpty(filter.FullName))
            {
                users = await _db.Users.Where(u => u.FullName.ToLower().Contains(filter.FullName.ToLower())).ToListAsync();
            }
            else if (filter.Date.HasValue)
            {
                users = await _db.Users.Where(u => DateTime.Compare(u.RegisteredAt, filter.Date.GetValueOrDefault()) == 0).ToListAsync();
            }
            else
            {
                users =  await _db.Users.ToListAsync();
            }

            if (filter.SortBy.HasValue)
            {
                ISorter<User> sorter = new UserSorter();
                return sorter.Sort(users, (byte)filter.SortBy, filter.SortOrder).ToList();
            }

            return users;
        }

        /// <inheritdoc cref="IUserRepository.Get(LoginData loginData)"/>
        public async Task<User?> Get(LoginData loginData)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email && u.Password == loginData.Password);
        }
        
        /// <inheritdoc cref="IUserRepository.Add(LoginData loginData)"/>
        public async Task Add(RegisterData registerData)
        {
            if (await _db.Users.FirstOrDefaultAsync(u => u.Email == registerData.Email && u.Password == registerData.Password) == null)
            {
                User user = new User 
                { 
                    FullName = registerData.FullName, 
                    Email = registerData.Email, 
                    Password = registerData.Password, 
                    RegisteredAt = DateTime.Today,
                    Role = Role.User 
                };
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new UserExistsException("Такой пользователь уже существует", 400);
            }
        }

        /// <inheritdoc cref="IUserRepository.GetById(int id)"/>
        public async Task<User?> GetById(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
