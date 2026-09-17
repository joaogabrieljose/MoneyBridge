using CustomerService.Domain.Modals;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Repository;

public class CustomerRepository : ICustumerRepository
{
    private readonly ConnectionContext _context;

    public CustomerRepository(ConnectionContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetByUserIdAsync(int userId)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Customer?> GetByTaxNumberAsync(string taxNumber)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.TaxNumber == taxNumber);
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public void Delete(Customer customer)
    {
        _context.Customers.Remove(customer);
    }

    public async Task<bool> ExistByEmailAsync(string email)
    {
        return await _context.Customers
            .AnyAsync(c => c.Email == email);
    }

    public async Task<bool> ExisteByTaxNumberAsync(string taxNumber)
    {
        return await _context.Customers
            .AnyAsync(c => c.TaxNumber == taxNumber);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}