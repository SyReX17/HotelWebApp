using HotelWebApp.Filters;
using HotelWebApp.Models;

namespace HotelWebApp.Interfaces.Services;

/// <summary>
/// Интерфейс сервиса для работы с комнатами
/// </summary>
public interface IRoomsService
{
    /// <summary>
    /// Получение комнат с использованием фильтров и сортировки
    /// </summary>
    /// <param name="filter">Фильтр комнат</param>
    /// <returns>Список комнат</returns>
    Task<List<HotelRoom>> GetAll(RoomFilter filter);

    /// <summary>
    /// Получение комнаты по идентификатору
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    /// <returns>Комната в виде объекта <c>HotelRoom</c></returns>
    Task<HotelRoom> GetById(int roomId);

    /// <summary>
    /// Получение списка свободных комнат
    /// </summary>
    /// <param name="filter">Фильтр для получения свободных комнат, содержит тип комнаты и даты проживания</param>
    /// <returns>Список свободных комнат</returns>
    Task<List<HotelRoom>> GetFreeRooms(BookingFilter filter);
}