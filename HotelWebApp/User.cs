using System.ComponentModel.DataAnnotations;

namespace HotelWebApp
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        public byte Role { get; set; }
    }
}
