using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Enums;
using HotelWebApp.Models;

namespace HotelWebApp;

/// <summary>
/// Класс инициализатора БД
/// </summary>
public class ProjectInitializer
{
    /// <summary>
    /// Инициализация БД, создание, миграция, добавление начальных данных, при создании
    /// </summary>
    /// <param name="context">Контекст подключения к БД</param>
    public static async Task Initialize(ApplicationContext context)
    {
        await context.Database.EnsureCreatedAsync();
        
        await context.Database.MigrateAsync();

        if (await context.Users.AnyAsync()) return;

        var admin = new User
        {
            Email = "admin@mail.ru",
            Password = BC.HashPassword("12345"),
            FullName = "Администратор",
            RegisteredAt = DateTime.Today,
            Role = Role.Admin
        };
        
        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();
        
        if (await context.Rooms.AnyAsync()) return;

        var types = new List<HotelRoomType>()
        {
            new HotelRoomType { Name = "Lite", Price = 199},
            new HotelRoomType { Name = "Middle", Price = 299},
            new HotelRoomType { Name = "High", Price = 499}
        };

        await context.RoomTypes.AddRangeAsync(types);
        await context.SaveChangesAsync();

        var rooms = new List<HotelRoom>()
        {
            new HotelRoom { Number = 101, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Number = 102, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Number = 103, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Number = 104, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Number = 105, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Number = 106, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Number = 107, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Number = 108, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Number = 201, Status = HotelRoomStatus.Free, Type = types[1]},
            new HotelRoom { Number = 202, Status = HotelRoomStatus.Free, Type = types[1]},
            new HotelRoom { Number = 203, Status = HotelRoomStatus.Free, Type = types[1]},
            new HotelRoom { Number = 204, Status = HotelRoomStatus.Free, Type = types[1]},
            new HotelRoom { Number = 301, Status = HotelRoomStatus.Free, Type = types[2]},
            new HotelRoom { Number = 302, Status = HotelRoomStatus.Free, Type = types[2]}
        };

        await context.Rooms.AddRangeAsync(rooms);
        await context.SaveChangesAsync();
    }
}