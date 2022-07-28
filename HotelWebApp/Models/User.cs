using System.ComponentModel.DataAnnotations;
using HotelWebApp.Enums;

namespace HotelWebApp
{
    /// <summary>
    /// Модель данных для пользователей
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [MaxLength(36)]
        public string Id { get; set; }
        
        /// <summary>
        /// Email пользователя
        /// </summary>
        [MaxLength(50)]
        public string Email { get; set; }
        
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [MaxLength(50)]
        public string Password { get; set; }
        
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// Дата и время регистрации пользователя
        /// </summary>
        public DateTime RegisteredAt { get; set; }
        
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public Role Role { get; set; }
    }
}
