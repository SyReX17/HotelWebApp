using HotelWebApp.Enums;
using HotelWebApp.Models;

namespace HotelWebApp.Sorting;

/// <summary>
/// Класс сортировщика для сортировки комнат,
/// реализует интерфейс <c>ISorter</c>
/// </summary>
public class RoomSorter : ISorter<HotelRoom>
{
    /// <inheritdoc cref="ISorter.Sort(IEnumerable<T> items, byte sortBy, SortOrder sortOrder)"/>
    public IEnumerable<HotelRoom> Sort(IEnumerable<HotelRoom> rooms, byte sortBy, SortOrder sortOrder)
    {
        if (sortBy == 0)
        {
            if (sortOrder == SortOrder.Reverse)
            {
                rooms = rooms.OrderByDescending(r => r.Number);
            }
            else
            {
                rooms = rooms.OrderBy(r => r.Number);
            }
        }
        else
        {
            if (sortOrder == SortOrder.Reverse)
            {
                rooms = rooms.OrderByDescending(r => r.Type.Price);
            }
            else
            {
                rooms = rooms.OrderBy(r => r.Type.Price);
            }
        }

        return rooms;
    }
}