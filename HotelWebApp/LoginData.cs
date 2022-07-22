using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelWebApp
{
    public class LoginData
    {
        [JsonPropertyName("email")]
        [Required]
        [EmailAddress]
        public string Email { get; } = null!;
        
        [JsonPropertyName("password")]
        [Required]
        public string Password { get; } = null!;
    }
}
