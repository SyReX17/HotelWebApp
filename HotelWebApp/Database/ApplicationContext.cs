using Microsoft.EntityFrameworkCore;
using HotelWebApp.Models;
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
    public DbSet<Booking> Bookings { get; set; } = null!;

    /// <summary>
    /// Хранит набор счетов на оплату
    /// </summary>
    public DbSet<Invoice> Invoices { get; set; } = null!;

    /// <summary>
    /// Конструктор контекста, принимает и передает настройки в базовый конструктор
    /// </summary>
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
}