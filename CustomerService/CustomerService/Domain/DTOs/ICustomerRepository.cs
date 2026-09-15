using CustomerService.Domain.Modals;

namespace CustomerService.Domain.DTOs;

public interface ICustomerRepository
{
    void Add(Customer customer);
    List<Customer> GetAll();
    Customer GetById(int id);
    
}