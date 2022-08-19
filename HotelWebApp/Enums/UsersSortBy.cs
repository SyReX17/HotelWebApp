namespace HotelWebApp.Enums;

/// <summary>
/// Параметры для сортировки пользователей
/// </summary>
public enum UsersSortBy : byte
{
    /// <summary>
    /// Полное имя пользователя (ФИО)
    /// </summary>
    Fullname,
    
    /// <summary>
    /// Дата регистрации пользователя
    /// </summary>
    Date
}