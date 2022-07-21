using System.ComponentModel.DataAnnotations;

namespace HotelWebApp
{
    public record class LoginData()
    {
        [EmailAddress]
        public string Email { get; }
        
        [Required]
        public string Password { get; }
    }
}
