using CustomerService.Domain.Modals;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure;

public class ConnectionContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Server=localhost;" +
            "port=5432;Database=customer_service;"+
            "User Id=postgres;"+
            "password=admin123;"); 
}
    
