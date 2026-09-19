using CustomerService.Domain.Modals;

namespace CustomerService.Application.Services;

public interface ICustomerService
{
    Task<Customer> CreateAsync(Customer customer);

    Task<Customer?> GetByIdAsync(int id);

    Task<List<Customer>> GetAllAsync();

    Task<Customer> UpdateAsync(int id, Customer customer);

    Task DeleteAsync(int id);
}