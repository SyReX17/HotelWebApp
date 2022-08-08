using HotelWebApp.Filters;
using HotelWebApp.Models;

namespace HotelWebApp.Repositories;

/// <summary>
/// Интерфейс для работы с бронирование комнат
/// </summary>
public interface IBookingRepository
{
    /// <summary>
    /// Добавление нового бронирования
    /// </summary>
    /// <param name="booking">Пользователь, комнаты, статус, даты</param>
    Task Add(Booking booking);

    /// <summary>
    /// Обновление статуса бронирования
    /// </summary>
    /// <param name="statusData">Данные о бронировании и новом статусе</param>
    Task UpdateStatus(StatusData statusData);

    /// <summary>
    /// Получение свободных комнат
    /// </summary>
    /// <param name="filter">Фильтр для получения свободных комнат</param>
    /// <returns>Список свободных комнат</returns>
    Task<List<HotelRoom>> GetFreeRooms(BookingFilter filter);

    /// <summary>
    /// Получения списка всех актуальных броней
    /// </summary>
    /// <returns>Список всех актуальных броней</returns>
    Task<List<Booking>> GetAll();

    /// <summary>
    /// Проверка окончания срока проживания
    /// </summary>
    Task CheckBookingEnding();

    /// <summary>
    /// Получение списка счетов на оплату
    /// </summary>
    /// <returns>Список счетов на оплату</returns>
    Task<List<Invoice>> GetInvoices();

    /// <summary>
    /// Подтверждение оплаты
    /// </summary>
    /// <param name="id">Идентификатор счета на оплату</param>
    Task ConfirmInvoice(int id);

    /// <summary>
    /// Продление бронирования
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя</param>
    /// <param name="bookingId">Идентификатор брони</param>
    /// <param name="newFinishAt">Новая дата окончания проживания</param>
    Task ExtendBooking(int UserId, int bookingId, DateTime newFinishAt);

    /// <summary>
    /// Отмена брони или принудительное завершение проживания
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя</param>
    /// <param name="bookingId">Идентификатор брони</param>
    Task RemoveBooking(int UserId, int bookingId);

    /// <summary>
    /// Проверка подтверждения броней
    /// </summary>
    Task CheckBookingConfirm();

    /// <summary>
    /// Принудительное выселение клиента
    /// </summary>
    /// <param name="userId">Идентификатор клиента</param>
    Task EvictClient(int userId);
}