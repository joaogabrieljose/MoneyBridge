using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.DTOs.Response;

using ResultPattern =
    global::CustomerService.Application.Common.Results;

namespace CustomerService.Application.Services;

public interface ICustomerService
{
    Task<ResultPattern.Results<CustomerResponse>> CreateAsync(
        CreateCustomerRequest customer);

    Task<ResultPattern.Results<CustomerResponse>> GetByIdAsync(
        int id);

    Task<ResultPattern.Results<List<CustomerResponse>>> GetAllAsync();

    Task<ResultPattern.Results<CustomerResponse>> UpdateAsync(
        int id,
        UpdateCustomerRequest customer);

    Task<ResultPattern.Result> DeleteAsync(
        int id);

    Task<ResultPattern.Results<List<CustomerResponse>>> GetPaginatedResultAsync(
        int pageNumber,
        int pageSize);
}