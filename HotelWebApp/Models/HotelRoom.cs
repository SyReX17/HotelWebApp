using HotelWebApp.Enums;

namespace HotelWebApp.Models
{
    /// <summary>
    /// Модель данных для комнат отеля
    /// </summary>
    public class HotelRoom
    {
        /// <summary>
        /// Идентификатор комнаты
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Номер комнаты
        /// </summary>
        public int Number { get; set; }
        
        /// <inheritdoc cref="HotelRoomStatus"/>
        public HotelRoomStatus? Status { get; set; }
        
        /// <inheritdoc cref="HotelRoomType"/>
        public HotelRoomType? Type { get; set; }
    }
}
