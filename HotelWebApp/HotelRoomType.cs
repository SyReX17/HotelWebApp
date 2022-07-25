using System.ComponentModel.DataAnnotations;

namespace HotelWebApp
{
    /// <summary>
    /// Модель данных для типов комнат в отеле
    /// </summary>
    public class HotelRoomType
    {
        /// <summary>
        /// Идентификатор типа комнаты
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Название типа комнаты
        /// </summary>
        [MaxLength(20)]
        public string Name { get; set; }
        
        /// <summary>
        /// Стоимость комнаты данного типа
        /// </summary>
        public decimal Price { get; set; }
    }
}
