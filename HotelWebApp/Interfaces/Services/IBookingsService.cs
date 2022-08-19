using HotelWebApp.Models;

namespace HotelWebApp.Interfaces.Services;

/// <summary>
/// Интерфейс сервиса для работы с бронями
/// </summary>
public interface IBookingsService
{
    /// <summary>
    /// Получения списка всех актуальных броней
    /// </summary>
    /// <returns>Список всех актуальных броней</returns>
    Task<List<Booking>> GetAll();

    /// <summary>
    /// Продление бронирования
    /// </summary>
    /// <param name="userEmail">Email пользователя</param>
    /// <param name="bookingId">Идентификатор брони</param>
    /// <param name="newFinishAt">Новая дата окончания проживания</param>
    Task ExtendBooking(string userEmail, int bookingId, DateTime newFinishAt);

    /// <summary>
    /// Подтверждение бронирования
    /// </summary>
    /// <param name="bookingId">Идентификатор бронирования</param>
    Task ConfirmBooking(int bookingId);

    /// <summary>
    /// Отмена брони или принудительное завершение проживания
    /// </summary>
    /// <param name="userEmail">Email пользователя</param>
    /// <param name="bookingId">Идентификатор бронирования</param>
    Task CancelBooking(string userEmail, int bookingId);

    /// <summary>
    /// Добавление нового бронирования
    /// </summary>
    /// <param name="data">Идетификатор комнаты, даты</param>
    /// <param name="userEmail">Email пользователя</param>
    Task<Booking> Add(UserBookingData data, string userEmail);
    
    /// <summary>
    /// Проверка окончания срока проживания
    /// </summary>
    Task CheckBookingEnding();
    
    /// <summary>
    /// Проверка подтверждения броней
    /// </summary>
    Task CheckBookingConfirm();

    /// <summary>
    /// Получение брони по идентификатору
    /// </summary>
    /// <param name="bookingId">Идентификатор брони</param>
    /// <returns>Бронь в виде объекта <c>Booking</c></returns>
    Task<Booking> GetById(int bookingId);

    /// <summary>
    /// Выселение клиетна
    /// </summary>
    /// <param name="bookingId">Идентификатор брони</param>
    Task EvictClient(int bookingId);
}