using HotelWebApp.Enums;
using HotelWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWebApp.Repositories;

/// <summary>
/// Класс репозитория для работы с БД,
/// реализующий интерфейс <c>IInvoicesRepository</c>
/// </summary>
public class InvoicesRepository : IInvoicesRepository
{
    /// <summary>
    /// Контекст подключения к БД
    /// </summary>
    private readonly ApplicationContext _db;

    /// <summary>
    /// Конструктор, принимает контекст подключения к БД
    /// </summary>
    /// <param name="context">Контекст подключения к БД</param>
    public InvoicesRepository(ApplicationContext context)
    {
        _db = context;
    }
    
    /// <inheritdoc cref="IInvoicesRepository.GetInvoices()"/>
    public async Task<List<Invoice>> GetInvoices()
    {
        return await _db.Invoices.Where(i => i.Status == InvoiceStatus.Awaiting).ToListAsync();
    }

    /// <inheritdoc cref="IInvoicesRepository.GetInvoiceById(int invoiceId)"/>
    public async Task<Invoice?> GetInvoiceById(int invoiceId)
    {
        return await _db.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId);
    }
    
    /// <inheritdoc cref="IInvoicesRepository.AddRange()"/>
    public async Task AddRange(List<Invoice> list)
    {
        await _db.Invoices.AddRangeAsync(list);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc cref="IInvoicesRepository.SaveChanges()"/>
    public async Task SaveChanges()
    {
        await _db.SaveChangesAsync();
    }
}