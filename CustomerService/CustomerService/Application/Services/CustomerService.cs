using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.DTOs.Response;
using CustomerService.Domain.Modals;
using CustomerService.Domain.Modals.Enums;
using CustomerService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ConnectionContext _connectionContext;

    public CustomerService(ConnectionContext connectionContext)
    {
        _connectionContext = connectionContext;
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
    {
        var emailExists = await _connectionContext.Customers
            .AnyAsync(c => c.Email == request.Email);

        if (emailExists)
        {
            throw new InvalidOperationException(
                "Já existe um cliente com esse email.");
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber))
        {
            var taxNumberExists = await _connectionContext.Customers
                .AnyAsync(c => c.TaxNumber == request.TaxNumber);

            if (taxNumberExists)
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

        await _connectionContext.Customers.AddAsync(customer);

        await _connectionContext.SaveChangesAsync();

        return MapToResponse(customer);
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id)
    {
        var customer = await _connectionContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer is null)
        {
            return null;
        }

        return MapToResponse(customer);
    }

    public async Task<List<CustomerResponse>> GetAllAsync()
    {
        var customers = await _connectionContext.Customers
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToListAsync();

        return customers
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<CustomerResponse> UpdateAsync(
        int id,
        UpdateCustomerRequest request)
    {
        var existingCustomer = await _connectionContext.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingCustomer is null)
        {
            throw new KeyNotFoundException(
                $"Cliente com ID {id} não encontrado.");
        }

        if (existingCustomer.Email != request.Email)
        {
            var emailExists = await _connectionContext.Customers
                .AnyAsync(c =>
                    c.Email == request.Email &&
                    c.Id != id);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "Já existe outro cliente com esse email.");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber)
            && existingCustomer.TaxNumber != request.TaxNumber)
        {
            var taxNumberExists = await _connectionContext.Customers
                .AnyAsync(c =>
                    c.TaxNumber == request.TaxNumber &&
                    c.Id != id);

            if (taxNumberExists)
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

        await _connectionContext.SaveChangesAsync();

        return MapToResponse(existingCustomer);
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _connectionContext.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer is null)
        {
            throw new KeyNotFoundException(
                $"Cliente com ID {id} não encontrado.");
        }

        _connectionContext.Customers.Remove(customer);

        await _connectionContext.SaveChangesAsync();
    }

    public async Task<List<CustomerResponse>> GetPaginatedResultAsync(
        int pageNumber,
        int pageSize)
    {
        if (pageNumber <= 0)
        {
            throw new ArgumentException(
                "PageNumber deve ser superior a zero.");
        }

        if (pageSize <= 0)
        {
            throw new ArgumentException(
                "PageSize deve ser superior a zero.");
        }

        var customers = await _connectionContext.Customers
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return customers
            .Select(MapToResponse)
            .ToList();
    }

    private static CustomerResponse MapToResponse(Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Gender = customer.Gender,
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