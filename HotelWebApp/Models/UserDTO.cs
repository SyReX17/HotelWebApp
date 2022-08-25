using HotelWebApp.Enums;

namespace HotelWebApp.Models;

/// <summary>
/// Модель данных для пользователей
/// </summary>
public class UserDTO
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public int Id { get; set; }
        
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
        
    /// <summary>
    /// ФИО пользователя
    /// </summary>
    public string FullName { get; set; }
        
    /// <summary>
    /// Дата и время регистрации пользователя
    /// </summary>
    public DateTime RegisteredAt { get; set; }
        
    /// <inheritdoc cref="Role"/>
    public Role Role { get; set; }
}