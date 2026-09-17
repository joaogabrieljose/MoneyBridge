namespace CustomerService.Domain.Modals;

public interface ICustumerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByUserIdAsync(int userId);
    Task<Customer?> GetByEmailAsync(string email);
    Task<List<Customer?>> GetAllAsync();
    Task<Customer?> GetByTaxNumberAsync(string taxNumber);
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
    Task<bool> ExistByEmailAsync(string email);
    Task<bool> ExisteByTaxNumberAsync(string taxNumber);
    Task SaveAsync();
    
}