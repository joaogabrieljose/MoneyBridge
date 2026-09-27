using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.DTOs.Response;
using CustomerService.Domain.Modals;
using CustomerService.Domain.Modals.Enums;
using CustomerService.Infrastructure;
using Microsoft.EntityFrameworkCore;

using ResultPattern = global::CustomerService.Application.Common.Results;

namespace CustomerService.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ConnectionContext _connectionContext;

    public CustomerService(ConnectionContext connectionContext)
    {
        _connectionContext = connectionContext;
    }

    // ============================================================
    // CREATE
    // ============================================================

    public async Task<ResultPattern.Results<CustomerResponse>> CreateAsync(
        CreateCustomerRequest request)
    {
        var emailExists = await _connectionContext.Customers
            .AnyAsync(c => c.Email == request.Email);

        if (emailExists)
        {
            return ResultPattern.Results<CustomerResponse>.Failure(
                ResultPattern.CustomerErrors.EmailAlreadyExists(
                    request.Email));
        }

        if (!string.IsNullOrWhiteSpace(request.TaxNumber))
        {
            var taxNumberExists = await _connectionContext.Customers
                .AnyAsync(c => c.TaxNumber == request.TaxNumber);

            if (taxNumberExists)
            {
                return ResultPattern.Results<CustomerResponse>.Failure(
                    ResultPattern.CustomerErrors.TaxNumberAlreadyExists(
                        request.TaxNumber));
            }
        }

        var customer = new Customer
        {
            FullName = request.FullName,
            Gender = request.Gender,
            Email = request.Email,
            Nationality = request.Nationality ?? string.Empty,
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

        var response = MapToResponse(customer);

        return ResultPattern.Results<CustomerResponse>.Success(response);
    }

    // ============================================================
    // GET BY ID
    // ============================================================

    public async Task<ResultPattern.Results<CustomerResponse>> GetByIdAsync(
        int id)
    {
        if (id <= 0)
        {
            return ResultPattern.Results<CustomerResponse>.Failure(
                ResultPattern.CustomerErrors.InvalidId(id));
        }

        var customer = await _connectionContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer is null)
        {
            return ResultPattern.Results<CustomerResponse>.Failure(
                ResultPattern.CustomerErrors.NotFound(id));
        }

        var response = MapToResponse(customer);

        return ResultPattern.Results<CustomerResponse>.Success(response);
    }

    // ============================================================
    // GET ALL
    // ============================================================

    public async Task<ResultPattern.Results<List<CustomerResponse>>> GetAllAsync()
    {
        var customers = await _connectionContext.Customers
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToListAsync();

        var response = customers
            .Select(MapToResponse)
            .ToList();

        return ResultPattern.Results<List<CustomerResponse>>
            .Success(response);
    }

    // ============================================================
    // UPDATE
    // ============================================================

    public async Task<ResultPattern.Results<CustomerResponse>> UpdateAsync(
        int id,
        UpdateCustomerRequest request)
    {
        if (id <= 0)
        {
            return ResultPattern.Results<CustomerResponse>.Failure(
                ResultPattern.CustomerErrors.InvalidId(id));
        }

        var existingCustomer = await _connectionContext.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingCustomer is null)
        {
            return ResultPattern.Results<CustomerResponse>.Failure(
                ResultPattern.CustomerErrors.NotFound(id));
        }

        /*
         * Se o email foi alterado, verificamos se já existe
         * noutro Customer.
         */
        if (existingCustomer.Email != request.Email)
        {
            var emailExists = await _connectionContext.Customers
                .AnyAsync(c =>
                    c.Email == request.Email &&
                    c.Id != id);

            if (emailExists)
            {
                return ResultPattern.Results<CustomerResponse>.Failure(
                    ResultPattern.CustomerErrors.EmailAlreadyExists(
                        request.Email));
            }
        }

        /*
         * Mesma regra para TaxNumber.
         *
         * Ignoramos TaxNumber vazio porque o campo é opcional.
         */
        if (!string.IsNullOrWhiteSpace(request.TaxNumber) &&
            existingCustomer.TaxNumber != request.TaxNumber)
        {
            var taxNumberExists = await _connectionContext.Customers
                .AnyAsync(c =>
                    c.TaxNumber == request.TaxNumber &&
                    c.Id != id);

            if (taxNumberExists)
            {
                return ResultPattern.Results<CustomerResponse>.Failure(
                    ResultPattern.CustomerErrors.TaxNumberAlreadyExists(
                        request.TaxNumber));
            }
        }

        existingCustomer.FullName = request.FullName;
        existingCustomer.Gender = request.Gender;
        existingCustomer.Email = request.Email;
        existingCustomer.phoneNumber = request.PhoneNumber;
        existingCustomer.TaxNumber = request.TaxNumber;
        existingCustomer.Nationality =
            request.Nationality ?? string.Empty;
        existingCustomer.DocumentNumber =
            request.DocumentNumber;

        existingCustomer.UpdatedAt = DateTime.UtcNow;

        await _connectionContext.SaveChangesAsync();

        var response = MapToResponse(existingCustomer);

        return ResultPattern.Results<CustomerResponse>.Success(response);
    }

    // ============================================================
    // DELETE
    // ============================================================

    public async Task<ResultPattern.Result> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return ResultPattern.Result.Failure(
                ResultPattern.CustomerErrors.InvalidId(id));
        }

        var customer = await _connectionContext.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer is null)
        {
            return ResultPattern.Result.Failure(
                ResultPattern.CustomerErrors.NotFound(id));
        }

        _connectionContext.Customers.Remove(customer);

        await _connectionContext.SaveChangesAsync();

        return ResultPattern.Result.Success();
    }

    // ============================================================
    // PAGINATION
    // ============================================================

    public async Task<ResultPattern.Results<List<CustomerResponse>>>
        GetPaginatedResultAsync(
            int pageNumber,
            int pageSize)
    {
        if (pageNumber <= 0)
        {
            return ResultPattern
                .Results<List<CustomerResponse>>
                .Failure(
                    ResultPattern.CustomerErrors.InvalidPageNumber(
                        pageNumber));
        }

        if (pageSize <= 0)
        {
            return ResultPattern
                .Results<List<CustomerResponse>>
                .Failure(
                    ResultPattern.CustomerErrors.InvalidPageSize(
                        pageSize));
        }

        var customers = await _connectionContext.Customers
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = customers
            .Select(MapToResponse)
            .ToList();

        return ResultPattern
            .Results<List<CustomerResponse>>
            .Success(response);
    }

    // ============================================================
    // MAPPER
    // ============================================================

    private static CustomerResponse MapToResponse(
        Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Gender = customer.Gender,
            Email = customer.Email,
            Nationality = customer.Nationality,
            Address = customer.Address,
            PhoneNumber = customer.phoneNumber,
            TaxNumber = customer.TaxNumber,
            Status = customer.Status,
            KycStatus = customer.KycStatus,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }
}