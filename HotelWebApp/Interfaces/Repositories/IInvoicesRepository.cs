using HotelWebApp.Models;

namespace HotelWebApp.Repositories;

public interface IInvoicesRepository
{
    /// <summary>
    /// Получение списка счетов на оплату
    /// </summary>
    /// <returns>Список счетов на оплату</returns>
    Task<List<Invoice>> GetInvoices();

    /// <summary>
    /// Получение счета на оплату по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор счета на оплату</param>
    /// <returns>Счет на оплату в виде обекта<c>Invoice</c></returns>
    Task<Invoice?> GetInvoiceById(int id);

    /// <summary>
    /// Добавление списка счетов на оплату
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task AddRange(List<Invoice> list);

    /// <summary>
    /// Сохранение изменений
    /// </summary>
    Task SaveChanges();
}