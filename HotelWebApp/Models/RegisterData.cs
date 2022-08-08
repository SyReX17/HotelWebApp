using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelWebApp
{
    /// <summary>
    /// Модель данных для для создания и входа в учетную запись пользователя
    /// </summary>
    public class RegisterData
    {
        /// <summary>
        /// Полное имя пользователя (ФИО)
        /// </summary>
        [JsonPropertyName("fullname")]
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        [JsonPropertyName("email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [JsonPropertyName("password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}