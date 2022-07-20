using System.ComponentModel.DataAnnotations;

namespace HotelWebApp
{
    public class User
    {
        [MaxLength(36)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        public byte Role { get; set; }
    }
}
