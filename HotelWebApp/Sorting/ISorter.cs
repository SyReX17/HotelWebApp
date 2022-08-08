using HotelWebApp.Enums;

namespace HotelWebApp.Sorting;

/// <summary>
/// Интерфейс для реализации сортировки по параметрам
/// </summary>
/// <typeparam name="T">Объект списка по которому будет производиться сортировка</typeparam>
public interface ISorter<T>
    where T : class
{
    /// <summary>
    /// Выполняет сортировку коллекции по выбранным параметрам и напрвлению сортировки
    /// </summary>
    /// <param name="items">Коллекция, которая будет сортироваться</param>
    /// <param name="sortBy">Параметр по которому будет сортироваться коллекция</param>
    /// <param name="sortOrder">Направление сортировки</param>
    /// <returns></returns>
    IEnumerable<T> Sort(IEnumerable<T> items, byte sortBy, SortOrder sortOrder);
}