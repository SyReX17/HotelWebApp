using HotelWebApp.Enums;

namespace HotelWebApp.Filters;

/// <summary>
/// Фильтр для поиска комнат
/// </summary>
public class RoomFilter
{
    /// <summary>
    /// Статус комнаты - параметр для фильтрации
    /// </summary>
    public HotelRoomStatus? Status { get; set; }
    
    /// <summary>
    /// Тип комнаты - параметр для фильтрации
    /// </summary>
    public RoomType? Type { get; set; }
    
    /// <summary>
    /// Параметры по которым может проходить сортировка
    /// </summary>
    public RoomsSortBy? SortBy { get; set; }
    
    /// <summary>
    /// Направление сортировки
    /// </summary>
    public SortOrder SortOrder { get; set; }
}