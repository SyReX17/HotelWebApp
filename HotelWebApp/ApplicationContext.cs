using Microsoft.EntityFrameworkCore;
using HotelWebApp;
public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<HotelRoom> Rooms { get; set; } = null!;
    public DbSet<HotelRoomType> RoomTypes { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=hotelappdb;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "admin@mail.ru", Password = "12345", Role = 1 }
        );
    }
}