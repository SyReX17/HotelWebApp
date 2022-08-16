using HotelWebApp.Enums;
using HotelWebApp.Filters;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Models;

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
        private ApplicationContext _db;

        public RoomsRepository(ApplicationContext context)
        {
            _db = context;
        }

        /// <inheritdoc cref="IRoomRepository.GetAll(UserFilter filter)"/>
        public async Task<List<HotelRoom>> GetAll(RoomFilter filter)
        {
            IQueryable<HotelRoom> query = _db.Rooms.Include(r => r.Type);

            if (filter.Status.HasValue)
            {
                query = query.Where(r => r.Status == filter.Status);
            }
            
            if (filter.Type.HasValue)
            {
                query = query.Where(r => r.Type.Id == (byte)filter.Type);
            }

            if (filter.SortBy.HasValue && filter.SortOrder.HasValue)
            {
                switch (filter.SortBy)
                {
                    case RoomsSortBy.Number:
                        query = (filter.SortOrder == SortOrder.Desc) 
                            ? query.OrderByDescending(r => r.Number) 
                            : query.OrderBy(r => r.Number);
                        break;
            
                    case RoomsSortBy.Price:
                        query = (filter.SortOrder == SortOrder.Desc) 
                            ? query.OrderByDescending(r => r.Type.Price) 
                            : query.OrderBy(r => r.Type.Price);
                        break;
                }
            }
            
            return await query.ToListAsync();
        }

        /// <inheritdoc cref="IRoomRepository.GetById(int Id)"/>
        public async Task<HotelRoom> GetById(int id)
        {
            var room = await _db.Rooms.Include(r => r.Type).FirstOrDefaultAsync(room => room.Id == id);
            return room;
        }
    }
}
