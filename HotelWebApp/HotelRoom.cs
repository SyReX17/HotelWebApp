namespace HotelWebApp
{
    public class HotelRoom
    {
        public int Id { get; set; }
        
        public int Number { get; set; }
        
        public byte Status { get; set; }
        
        public HotelRoomType? Type { get; set; }
    }
}
