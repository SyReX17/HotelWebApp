using Microsoft.EntityFrameworkCore;

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
        
        /// <summary>
        /// Возвращает пользовалеля по его данным
        /// </summary>
        /// <param name="loginData">
        /// Email и пароль пользователя
        /// </param>
        /// <returns>
        /// Пользователя в виде объекта <c>User</c>
        /// </returns>
        public async Task<User?> Get(LoginData loginData)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email && u.Password == loginData.Password);
        }
        
        /// <summary>
        /// Добавляет нового пользователя
        /// </summary>
        /// <param name="loginData">
        /// Email и пароль пользователя
        /// </param>
        public async Task Add(LoginData loginData)
        {
            if (await _db.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email && u.Password == loginData.Email) == null)
            {
                User user = new User { Id = Guid.NewGuid().ToString(), Email = loginData.Email, Password = loginData.Password, Role = (byte)Role.User };
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
            }
        }
        
        /// <summary>
        /// Отслеживаем, был ли вызван Dispose.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Метод для очистки используемых ресурсов
        /// </summary>
        /// <param name="disposing"></param>
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
        }

        /// <summary>
        /// Реализация интерфейса IDisposable,
        /// вызов освобождения ресурсов, сигнал GB
        /// для предотвращения повторного
        /// освобождения ресурсов
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
