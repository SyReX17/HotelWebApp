namespace HotelWebApp.Repositories;

/// <summary>
/// Интерфейс для работы с бронирование комнат
/// </summary>
public interface IBookingsRepository
{
    /// <summary>
    /// Добавление нового бронирования
    /// </summary>
    /// <param name="booking">Пользователь, комнаты, статус, даты</param>
    Task Add(Booking booking);

    /// <summary>
    /// Получения списка всех актуальных броней
    /// </summary>
    /// <returns>Список всех актуальных броней</returns>
    Task<List<Booking>> GetAll();

    /// <summary>
    /// Отмена брони или принудительное завершение проживания
    /// </summary>
    /// <param name="bookingId">Идентификатор брони</param>
    Task Remove(Booking booking);

    /// <summary>
    /// Получение брони по идентификатору
    /// </summary>
    /// <param name="bookingId">Идентификатор брони</param>
    /// <returns>Бронь в виде объекта <c>Booking</c></returns>
    Task<Booking?> GetById(int bookingId);

    /// <summary>
    /// Получение списка неподтвержденных броней
    /// </summary>
    /// <returns>Список неподтвержденных броней</returns>
    Task<List<Booking>> GetUnconfirmedBookings();

    /// <summary>
    /// Удаление списка броней
    /// </summary>
    /// <param name="list">Список броней</param>
    Task RemoveRange(List<Booking> list);

    /// <summary>
    /// Получение списка завершенных броней
    /// </summary>
    /// <returns>Список завершенных броней</returns>
    Task<List<Booking>> GetComletedBookings();
    
    /// <summary>
    /// Сохранение изменений
    /// </summary>
    /// <returns></returns>
    Task SaveChanges();
}