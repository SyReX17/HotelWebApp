namespace HotelWebApp
{
    public class Session
    {
        public int Id { get; set; }
        
        public User? Resident { get; set; }
        
        public HotelRoom? Room { get; set; }
        
        public bool Confirm { get; set; }
        
        public DateTime? StartAt { get; set; }
        
        public DateTime? FinishAt { get; set; }
    }
}
