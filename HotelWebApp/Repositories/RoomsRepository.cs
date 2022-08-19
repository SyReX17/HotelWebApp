using HotelWebApp.Enums;
using HotelWebApp.Filters;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Models;
using HotelWebApp.Sorting;

namespace HotelWebApp.Repositories
{
    /// <summary>
    /// Класс репозитория для взаимодействия с БД,
    /// реализует интерфейс <c>IRoomRepository</c>
    /// </summary>
    public class RoomsRepository : IRoomRepository
    {
        /// <summary>
        /// Контекст подключения к БД
        /// </summary>
        private ApplicationContext _db = new ApplicationContext();

        /// <inheritdoc cref="IRoomRepository.GetAll(UserFilter filter)"/>
        public async Task<List<HotelRoom>> GetAll(RoomFilter filter)
        {
            List<HotelRoom> rooms;

            if (filter.Status.HasValue && filter.Type.HasValue)
            {
                rooms = await _db.Rooms.Include(r => r.Type)
                    .Where(r => r.Status == filter.Status && r.Type.Id == (byte)filter.Type)
                    .ToListAsync();
            }
            else if (filter.Status.HasValue)
            {
                rooms = await _db.Rooms.Include(r => r.Type)
                    .Where(r => r.Status == filter.Status).ToListAsync();
            }
            else if (filter.Type.HasValue)
            {
                rooms = await _db.Rooms.Include(r => r.Type)
                    .Where(r => r.Type.Id == (byte)filter.Type).ToListAsync();
            }
            else
            {
                rooms = await _db.Rooms.Include(r => r.Type).ToListAsync();
            }

            if (filter.SortBy.HasValue)
            {
                ISorter<HotelRoom> sorter = new RoomSorter();
                return sorter.Sort(rooms, (byte)filter.SortBy, filter.SortOrder).ToList();  
            }

            return rooms;
        }

        /// <inheritdoc cref="IRoomRepository.GetById(int Id)"/>
        public async Task<HotelRoom> GetById(int id)
        {
            var room = await _db.Rooms.Include(r => r.Type).FirstOrDefaultAsync(room => room.Id == id);
            return room;
        }
    }
}
