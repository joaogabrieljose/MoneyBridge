namespace CustomerService.Application.DTOs.Requests;

public class CreateCustomerRequest
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? Nationality { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? TaxNumber { get; set; }

    public string? DocumentNumber { get; set; }
}