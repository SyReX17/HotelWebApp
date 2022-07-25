namespace HotelWebApp
{
    /// <summary>
    /// состояние комнаты свободна/занята/на ремонте
    /// </summary>
    public enum HotelRoomStatus : byte
    {
        Free,
        
        Occupied,
        
        Repair
    }
}
