using System.ComponentModel.DataAnnotations;

namespace HotelWebApp
{
    public class HotelRoomType
    {
        public int Id { get; set; }
        
        [MaxLength(20)]
        public string Name { get; set; }
        
        public decimal Price { get; set; }
    }
}
