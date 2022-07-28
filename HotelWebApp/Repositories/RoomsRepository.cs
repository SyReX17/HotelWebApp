﻿using HotelWebApp.Enums;
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
    public class RoomsRepository : IRoomRepository, IDisposable
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

        /// <inheritdoc cref="IRoomRepository.GetById(string Id)"/>
        public async Task<HotelRoom> GetById(string id)
        {
            var room = await _db.Rooms.Include(r => r.Type).FirstOrDefaultAsync(room => room.Id == id);
            return room;
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
