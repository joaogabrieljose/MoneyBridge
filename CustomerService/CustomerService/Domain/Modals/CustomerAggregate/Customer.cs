using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerService.Domain.Modals.Enums;

namespace CustomerService.Domain.Modals;

[Table("customer")]
public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string Nationality { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public string phoneNumber { get; set; }
    public string? TaxNumber { get; set; }
    public string? DocumentNumber { get; set; }
    public DocumentType? DocumentType { get; set; }
    public CustomerStatus Status { get; set; }
    public KycStatus KycStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Customer() { }

    public Customer(int id, int userId, string fullName, string gender, string email, string nationality, string address, int age, string phoneNumber, string? taxNumber, string? documentNumber, DocumentType? documentType, CustomerStatus status, KycStatus kycStatus, DateTime createdAt, DateTime? updatedAt)
    {
        this.Id = id;
        UserId = userId;
        FullName = fullName;
        Gender = gender;
        Email = email;
        Nationality = nationality;
        Address = address;
        Age = age;
        this.phoneNumber = phoneNumber;
        TaxNumber = taxNumber;
        DocumentNumber = documentNumber;
        DocumentType = documentType;
        Status = status;
        KycStatus = kycStatus;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}