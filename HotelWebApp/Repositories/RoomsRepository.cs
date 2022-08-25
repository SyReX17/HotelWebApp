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
    public class RoomsRepository : IRoomsRepository
    {
        /// <summary>
        /// Контекст подключения к БД
        /// </summary>
        private ApplicationContext _db;

        /// <summary>
        /// Конструктор, принимает контекст подключения к БД
        /// </summary>
        /// <param name="context">Контекст подключения к БД</param>
        public RoomsRepository(ApplicationContext context)
        {
            _db = context;
        }

        /// <inheritdoc cref="IRoomsRepository.GetAll(UserFilter filter)"/>
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

        /// <inheritdoc cref="IRoomsRepository.GetById(int Id)"/>
        public async Task<HotelRoom?> GetById(int id)
        {
            return await _db.Rooms.Include(r => r.Type).FirstOrDefaultAsync(room => room.Id == id);;
        }
        
        /// <inheritdoc cref="IRoomsRepository.GetFreeRooms(BookingFilter filter)"/>
        public async Task<List<HotelRoom>> GetFreeRooms(BookingFilter filter)
        {
            return await _db.Rooms.Include(r => r.Type).Where(r =>  !(_db.Bookings
                .Where(b => (filter.StartAt >= b.StartAt && filter.StartAt <= b.FinishAt) || (filter.FinishAt >= b.StartAt && 
                    filter.FinishAt <= b.FinishAt) || (filter.StartAt <= b.StartAt && filter.FinishAt >= b.FinishAt))
                .Select(b => b.RoomId).ToList().Contains(r.Id)) && r.Type.Id == (int)filter.Type).ToListAsync();
        }
    }
}
