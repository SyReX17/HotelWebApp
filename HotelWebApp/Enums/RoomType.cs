namespace HotelWebApp.Enums;

/// <summary>
/// Типы комнат
/// </summary>
public enum RoomType : byte
{
    /// <summary>
    /// Комната низкого уровня
    /// </summary>
    Lite = 1,
    
    /// <summary>
    /// Комната среднего уровня
    /// </summary>
    Middle,
    
    /// <summary>
    /// Комната высокого уровня
    /// </summary>
    High
}