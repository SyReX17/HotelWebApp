using HotelWebApp.Models;

namespace HotelWebApp.Interfaces.Services;

/// <summary>
/// Интерфейс сервиса для работы с счетами на оплату
/// </summary>
public interface IInvoicesService
{
    /// <summary>
    /// Получение списка счетов на оплату
    /// </summary>
    /// <returns>Список счетов на оплату</returns>
    Task<List<Invoice>> GetInvoices();

    /// <summary>
    /// Подтверждение оплаты
    /// </summary>
    /// <param name="invoiceId">Идентификатор счета на оплату</param>
    Task ConfirmInvoice(int invoiceId);
}