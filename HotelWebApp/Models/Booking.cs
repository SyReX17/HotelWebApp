using HotelWebApp.Enums;

namespace HotelWebApp
{
    /// <summary>
    /// Модель данных для брони комнаты
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Идентификатор брони
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int ResidentId { get; set; }
        
        /// <summary>
        /// Идентификатор комнаты
        /// </summary>
        public int RoomId { get; set; }
        
        /// <inheritdoc cref="BookingStatus"/>
        public BookingStatus Status { get; set; }
        
        /// <summary>
        /// Дата начала проживания
        /// </summary>
        public DateTime? StartAt { get; set; }
        
        /// <summary>
        /// Дата окончания проживания
        /// </summary>
        public DateTime? FinishAt { get; set; }
    }
}
