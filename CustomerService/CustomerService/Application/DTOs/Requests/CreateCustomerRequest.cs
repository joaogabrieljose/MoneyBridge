namespace CustomerService.Application.DTOs.Requests;

public class CreateCustomerRequest
{
    public string FullName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string  Email { get; set; }

    public string? Nationality { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set;  }
    
    public int Age { get; set; }
    
    public string? TaxNumber { get; set; }

    public string? DocumentNumber { get; set; }
}