using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.DTOs.Response;
using CustomerService.Domain.Modals;

namespace CustomerService.Application.Services;

public interface ICustomerService
{
    Task<CustomerResponse> CreateAsync(CreateCustomerRequest customer);

    Task<CustomerResponse?> GetByIdAsync(int id);

    Task<List<CustomerResponse>> GetAllAsync();

    Task<CustomerResponse> UpdateAsync(int id, UpdateCustomerRequest customer);

    Task DeleteAsync(int id);
}