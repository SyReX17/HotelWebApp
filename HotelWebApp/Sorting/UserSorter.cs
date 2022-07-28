using HotelWebApp.Enums;

namespace HotelWebApp.Sorting;

/// <summary>
/// Класс сортировщика для сортировки пользователей,
/// реализует интерфейс <c>ISorter</c>
/// </summary>
public class UserSorter : ISorter<User>
{
    /// <inheritdoc cref="ISorter.Sort(IEnumerable<T> items, byte sortBy, SortOrder sortOrder)"/>
    public IEnumerable<User> Sort(IEnumerable<User> users, byte sortBy, SortOrder sortOrder)
    {
        if (sortBy == 0)
        {
            if (sortOrder == SortOrder.Reverse)
            {
                users = users.OrderByDescending(u => u.FullName).ToList();
            }
            else
            {
                users = users.OrderBy(u => u.FullName).ToList();
            }
        }
        else
        {
            if (sortOrder == SortOrder.Reverse)
            {
                users = users.OrderByDescending(u => u.FullName).ToList();
            }
            else
            {
                users = users.OrderBy(u => u.FullName).ToList();
            }
        }

        return users;
    }
}