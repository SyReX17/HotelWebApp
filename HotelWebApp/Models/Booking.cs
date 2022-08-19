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

        /// <summary>
        /// Подтверждение брони
        /// </summary>
        public void Confirm()
        {
            Status = BookingStatus.Confirm;
        }

        /// <summary>
        /// Остановка проживания
        /// </summary>
        public void Stop()
        {
            FinishAt = DateTime.Today.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
        }
        
        /// <summary>
        /// Получение стоимости проживания
        /// </summary>
        /// <param name="basePrice">Базавая стоимость проживания</param>
        /// <returns>Стоимость проживания за определенный промежуток времени</returns>
        public decimal GetPrice(decimal basePrice)
        {
            var diffTime = FinishAt.Value.Subtract(StartAt.Value);

            return Convert.ToDecimal(diffTime.TotalMinutes) * (basePrice / 60);
        }
    }
}
