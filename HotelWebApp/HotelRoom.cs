namespace HotelWebApp
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
        
        /// <summary>
        /// Статус состояния комнаты
        /// </summary>
        public byte Status { get; set; }
        
        /// <summary>
        /// Тип комнаты
        /// </summary>
        public HotelRoomType? Type { get; set; }
    }
}
