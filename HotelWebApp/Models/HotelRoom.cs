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
        public string Id { get; set; }
        
        /// <summary>
        /// Номер комнаты
        /// </summary>
        public int Number { get; set; }
        
        /// <summary>
        /// Статус состояния комнаты
        /// </summary>
        public HotelRoomStatus? Status { get; set; }
        
        /// <summary>
        /// Внешний ключ для типов комнат
        /// </summary>
        public int TypeId { get; set; }
        
        /// <summary>
        /// Тип комнаты
        /// </summary>
        public HotelRoomType? Type { get; set; }
    }
}
