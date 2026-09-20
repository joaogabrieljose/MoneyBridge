using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.DTOs.Response;
using CustomerService.Domain.Modals;
using CustomerService.Domain.Modals.Enums;

namespace CustomerService.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustumerRepository _customerRepository;

    public CustomerService(ICustumerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
    {
        var emailExists =
            await _customerRepository.GetByEmailAsync(request.Email);

        if (emailExists is not null)
        {
            throw new InvalidOperationException(
                "Já existe um cliente com esse email.");
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber))
        {
            var taxNumberExists =
                await _customerRepository
                    .GetByTaxNumberAsync(request.TaxNumber);

            if (taxNumberExists is not null)
            {
                throw new InvalidOperationException(
                    "Já existe um cliente com esse número fiscal.");
            }
        }

        var customer = new Customer
        {
            FullName = request.FullName,
            Gender = request.Gender,
            Email = request.Email,
            Nationality = request.Nationality,
            phoneNumber = request.PhoneNumber,
            Address = request.Address,
            Age = request.Age,
            TaxNumber = request.TaxNumber,
            DocumentType = DocumentType.IdentityCard,
            DocumentNumber = request.DocumentNumber,
            Status = CustomerStatus.Active,
            KycStatus = KycStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _customerRepository.AddAsync(customer);

        await _customerRepository.SaveAsync();

        return MapToResponse(customer);
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id)
    {
        var customer =
            await _customerRepository.GetByIdAsync(id);

        if (customer is null)
        {
            return null;
        }

        return MapToResponse(customer);
    }

    public async Task<List<CustomerResponse>> GetAllAsync()
    {
        var customers =
            await _customerRepository.GetAllAsync();

        return customers
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<CustomerResponse> UpdateAsync(
        int id,
        UpdateCustomerRequest request)
    {
        var existingCustomer =
            await _customerRepository.GetByIdAsync(id);

        if (existingCustomer is null)
        {
            throw new KeyNotFoundException(
                $"Cliente com ID {id} não encontrado.");
        }

        if (existingCustomer.Email != request.Email)
        {
            var emailExists =
                await _customerRepository.GetByEmailAsync(request.Email);

            if (emailExists is not null)
            {
                throw new InvalidOperationException(
                    "Já existe outro cliente com esse email.");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber)
            && existingCustomer.TaxNumber != request.TaxNumber)
        {
            var taxNumberExists =
                await _customerRepository
                    .GetByTaxNumberAsync(request.TaxNumber);

            if (taxNumberExists is not null)
            {
                throw new InvalidOperationException(
                    "Já existe outro cliente com esse número fiscal.");
            }
        }

        existingCustomer.FullName = request.FullName;
        existingCustomer.Gender = request.Gender;
        existingCustomer.Email = request.Email;
        existingCustomer.phoneNumber = request.PhoneNumber;
        existingCustomer.TaxNumber = request.TaxNumber;
        existingCustomer.Nationality = request.Nationality;
        existingCustomer.DocumentNumber = request.DocumentNumber;
        existingCustomer.UpdatedAt = DateTime.UtcNow;

        _customerRepository.Update(existingCustomer);

        await _customerRepository.SaveAsync();

        return MapToResponse(existingCustomer);
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

    private static CustomerResponse MapToResponse(Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Gender = customer.Gender.ToString(),
            Email = customer.Email,
            PhoneNumber = customer.phoneNumber,
            TaxNumber = customer.TaxNumber,
            Nationality = customer.Nationality,
            Status = customer.Status,
            KycStatus = customer.KycStatus,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }
}