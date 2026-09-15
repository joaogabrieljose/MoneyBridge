using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerService.Domain.Modals;

[Table("customer")]
public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string Nationality { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public int phoneNumber { get; set; }

    public Customer()
    {
    }

    public Customer(string fullName, string gender, string email, string nationality, string address, int age, int phoneNumber)
    {
        FullName = fullName;
        Gender = gender;
        Email = email;
        Nationality = nationality;
        Address = address;
        Age = age;
        this.phoneNumber = phoneNumber;
    }
}