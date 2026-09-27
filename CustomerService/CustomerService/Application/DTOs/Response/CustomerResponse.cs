using CustomerService.Domain.Modals;
using CustomerService.Domain.Modals.Enums;

namespace CustomerService.Application.DTOs.Response;

public class CustomerResponse
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Gender { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    public string? Nationality { get; set; }

    public string Address {get; set; }

    public string PhoneNumber { get; set; }
    
    public string? TaxNumber { get; set; }

    public CustomerStatus Status { get; set; }

    public KycStatus KycStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
}