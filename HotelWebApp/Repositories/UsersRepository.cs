using Microsoft.EntityFrameworkCore;
using HotelWebApp.Enums;
using HotelWebApp.Filters;

namespace HotelWebApp.Repositories
{
    /// <summary>
    /// Класс репозитория для взаимодействия с БД,
    /// реализует интерфейс <c>IUserRepository</c>
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        /// <summary>
        /// Контекст подключения к БД
        /// </summary>
        private ApplicationContext _db;

        /// <summary>
        /// Конструктор, принимет контекст подключения к БД
        /// </summary>
        /// <param name="context">Котекст подключения к БД</param>
        public UsersRepository(ApplicationContext context)
        {
            _db = context;
        }

        /// <inheritdoc cref="IUsersRepository.GetAll(UserFilter filter)"/>
        public async Task<List<User>> GetAll(UserFilter filter)
        {
            IQueryable<User> query = _db.Users;
            
            if (!String.IsNullOrEmpty(filter.FullName))
            {
                query = query.Where(u => u.FullName.ToLower().Contains(filter.FullName.ToLower()));
            }
            
            if (filter.Date.HasValue)
            {
                query = query.Where(u => DateTime.Compare(u.RegisteredAt, filter.Date.GetValueOrDefault()) == 0);
            }

            if (filter.SortBy.HasValue && filter.SortOrder.HasValue)
            {
                switch (filter.SortBy)
                {
                    case UserSortBy.Fullname:
                        query = (filter.SortOrder == SortOrder.Desc)
                            ? query.OrderByDescending(u => u.FullName)
                            : query.OrderBy(u => u.FullName);
                        break;

                    case UserSortBy.Date:
                        query = (filter.SortOrder == SortOrder.Desc)
                            ? query.OrderByDescending(u => u.RegisteredAt)
                            : query.OrderBy(u => u.RegisteredAt);
                        break;
                }
            }
            
            return await query.ToListAsync();
        }
        
        /// <inheritdoc cref="IUsersRepository.Add(LoginData loginData)"/>
        public async Task Add(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc cref="IUsersRepository.GetById(int id)"/>
        public async Task<User?> GetById(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc cref="IUsersRepository.GetByEmail(string email)"/>
        public async Task<User?> GetByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
