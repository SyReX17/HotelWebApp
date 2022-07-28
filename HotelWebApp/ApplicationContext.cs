using Microsoft.EntityFrameworkCore;
using HotelWebApp;

/// <summary>
/// Контекст подключения к БД
/// </summary>
public class ApplicationContext : DbContext
{
    /// <summary>
    /// Хранит набор данных типа <c>User</c>
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;
    
    /// <summary>
    /// Хранит набор данных типа <c>Room</c>
    /// </summary>
    public DbSet<HotelRoom> Rooms { get; set; } = null!;
    
    /// <summary>
    /// Хранит набор данных типа <c>RoomType</c>
    /// </summary>
    public DbSet<HotelRoomType> RoomTypes { get; set; } = null!;
    
    /// <summary>
    /// Хранит набор данных типа <c>Session</c>
    /// </summary>
    public DbSet<Session> Sessions { get; set; } = null!;
    
    /// <summary>
    /// Конструктор контекста, создает БД, если она не создана
    /// </summary>
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Переопределение метода <c>OnConfiguring</c> для установления подключения к БД
    /// </summary>
    /// <param name="optionsBuilder">Параметр для настройки строки подключения к БД</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=hotelappdb;Trusted_Connection=True;");
    }
    
    /// <summary>
    /// Устанавливает начальные данные в БД, при её создании
    /// </summary>
    /// <param name="modelBuilder">Параметр для установления начальных значений в БД</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
                new User { Id = Guid.NewGuid().ToString(), Email = "admin@mail.ru", Password = "12345", Role = 0 }
        );
    }
}