using HotelWebApp.Filters;
using HotelWebApp.Models;

namespace HotelWebApp.Repositories;

/// <summary>
/// Интерфейс для работы с репозиторием пользователей
/// </summary>
public interface IRoomRepository
{
    /// <summary>
    /// Возвращает список комнат с использованием фильтров и сортировки
    /// </summary>
    /// <param name="filter">Фильтра для поиска комнат</param>
    /// <returns>Список комнат</returns>
    Task<List<HotelRoom>> GetAll(RoomFilter filter);

    /// <summary>
    /// Возвращает комнату по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор комнаты</param>
    /// <returns>Комната</returns>
    Task<HotelRoom> GetById(int id);
}