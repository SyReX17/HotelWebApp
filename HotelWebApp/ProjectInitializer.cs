using Microsoft.EntityFrameworkCore;
using HotelWebApp.Enums;
using HotelWebApp.Models;

namespace HotelWebApp;

/// <summary>
/// Класс инициализатора БД
/// </summary>
public static class ProjectInitializer
{
    /// <summary>
    /// Инициализация БД, создание, миграция, добавление начальных данных, при создании
    /// </summary>
    /// <param name="context">Контекст подключения к БД</param>
    public static async void Initialize(ApplicationContext context)
    {
        await context.Database.EnsureCreatedAsync();
        
        await context.Database.MigrateAsync();

        if (await context.Users.AnyAsync()) return;

        var admin = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@mail.ru",
            Password = "12345",
            FullName = "Администратор",
            RegisteredAt = DateTime.Today,
            Role = (byte)Role.Admin
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
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 101, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 102, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 103, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 104, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 105, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 106, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 107, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 108, Status = HotelRoomStatus.Free, Type = types[0]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 201, Status = HotelRoomStatus.Free, Type = types[1]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 202, Status = HotelRoomStatus.Free, Type = types[1]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 203, Status = HotelRoomStatus.Free, Type = types[1]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 204, Status = HotelRoomStatus.Free, Type = types[1]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 301, Status = HotelRoomStatus.Free, Type = types[2]},
            new HotelRoom { Id = Guid.NewGuid().ToString(), Number = 302, Status = HotelRoomStatus.Free, Type = types[2]}
        };

        await context.AddRangeAsync(rooms);
        await context.SaveChangesAsync();
    }
}