using HotelWebApp.Enums;

namespace HotelWebApp.Filters;

/// <summary>
/// Фильтр для поиска пользователей
/// </summary>
public class UserFilter
{
    /// <summary>
    /// Полное имя пользователя (ФИО)
    /// </summary>
    public string? FullName { get; set; }
    
    /// <summary>
    /// Дата регистрации пользователя - параметр для фильтрации
    /// </summary>
    public DateTime? Date { get; set; }
    
    /// <summary>
    /// Параметры по которым может проходить сортировка
    /// </summary>
    public UsersSortBy? SortBy { get; set; }
    
    /// <summary>
    /// Направление сортировки
    /// </summary>
    public SortOrder SortOrder { get; set; }
}