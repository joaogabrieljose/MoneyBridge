using CustomerService.Domain.Modals;

namespace CustomerService.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustumerRepository _customerRepository;

    public CustomerService(ICustumerRepository  customerRepository)
    {
        _customerRepository = customerRepository;
    }



    public async Task<Customer> CreateAsync(Customer customer)
    {
        var emailExistes = await _customerRepository.GetByEmailAsync(customer.Email);
        if (emailExistes != null)
        {
            throw new Exception("já existe cliente com esse email");
        }

        if (!string.IsNullOrWhiteSpace(customer.TaxNumber))
        {
            var taxNumberExistes = await _customerRepository.GetByTaxNumberAsync(customer.TaxNumber);
            if (taxNumberExistes != null)
            {
                throw new InvalidOperationException("já existem cliente com esse número fiscal ");
            }

        }
        
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveAsync();
        return customer;
    }
    

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _customerRepository.GetAllAsync();
    }
    

    public async Task<Customer> UpdateAsync(int id, Customer customer)
    {
        var existemCustomer = _customerRepository.GetByIdAsync(id);
        if (existemCustomer is null)
        {
            throw new KeyNotFoundException($"Cliente com {id} não encontrado.");
        }
        _customerRepository.Update(customer);
        await _customerRepository.SaveAsync();
        return customer;
    }
    
    

    
    public async Task DeleteAsync(int id)
    {
        var customer =
            await _customerRepository.GetByIdAsync(id);

        if (customer is null)
        {
            throw new KeyNotFoundException(
                $"Cliente com ID {id} não encontrado.");
        }

        _customerRepository.Delete(customer);

        await _customerRepository.SaveAsync();
    }
}