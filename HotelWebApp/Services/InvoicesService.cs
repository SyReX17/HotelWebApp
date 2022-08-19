using HotelWebApp.Exceptions;
using HotelWebApp.Interfaces.Services;
using HotelWebApp.Models;
using HotelWebApp.Repositories;

namespace HotelWebApp.Services;

public class InvoicesService : IInvoicesService
{
    /// <summary>
    /// Интерфейс репозитория для работы с счетами на оплату
    /// </summary>
    private readonly IInvoicesRepository _repository;

    public InvoicesService(IInvoicesRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc cref="IInvoicesService.GetInvoices()"/>
    public async Task<List<Invoice>> GetInvoices()
    {
        return await _repository.GetInvoices();
    }

    /// <inheritdoc cref="IInvoicesService.ConfirmInvoice(int invoiceId)"/>
    public async Task ConfirmInvoice(int invoiceId)
    {
        var invoice = await _repository.GetInvoiceById(invoiceId);

        if (invoice == null) throw new InvoiceNotFoundException();
        
        invoice.Confirm();

        await _repository.Update(invoice);
    }
}