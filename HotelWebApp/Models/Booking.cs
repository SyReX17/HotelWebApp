using HotelWebApp.Enums;
using HotelWebApp.Models;

namespace HotelWebApp
{
    public class Booking
    {
        public string Id { get; set; }
        
        public User? Resident { get; set; }
        
        public HotelRoomType? RoomType { get; set; }
        
        public BookingStatus Status { get; set; }
        
        public DateTime? StartAt { get; set; }
        
        public DateTime? FinishAt { get; set; }
    }
}
