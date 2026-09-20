using CustomerService.Domain.Modals;
using CustomerService.Domain.Modals.Enums;
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

    public List<Customer> getPaginatedResult(int pageNumber, int pageQuantity)
    {
        return _context.Customers
            .Skip(pageNumber * pageQuantity)
            .Take(pageQuantity)
            .Select(b => new Customer()
            {
                Id = b.Id,
                FullName = b.FullName,
                Gender = b.Gender,
                Email = b.Email,
                Nationality = b.Nationality, 
                Address = b.Address,
                Age = b.Age,
                phoneNumber = b.phoneNumber,
                TaxNumber = b.TaxNumber,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                DocumentType = new DocumentType(),
                
            })
            .ToList();
        
    }
}