using Microsoft.EntityFrameworkCore;

namespace HotelWebApp
{
    public static class UsersRepository
    {
        public async static Task<User?> GetUserAsync(string email, string password)
        {
            using (var db = new ApplicationContext())
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            }
        }

        public async static Task AddUserAsync(string email, string password)
        {
            using (var db = new ApplicationContext())
            {
                if (await db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password) == null)
                {
                    User user = new User() { Id = Guid.NewGuid().ToString(), Email = email, Password = password, Role = (byte)Role.User };
                    await db.Users.AddAsync(user);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
