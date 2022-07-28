namespace HotelWebApp.Enums
{
    /// <summary>
    /// Cостояние комнаты
    /// </summary>
    public enum HotelRoomStatus : byte
    {
        /// <summary>
        /// Комната свободна
        /// </summary>
        Free,
        
        /// <summary>
        /// Комната занята
        /// </summary>
        Occupied,
        
        /// <summary>
        /// Комната на ремонте
        /// </summary>
        Repair
    }
}
