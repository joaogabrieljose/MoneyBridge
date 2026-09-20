using CustomerService.Domain.Modals;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure;

public class ConnectionContext : DbContext
{
    public ConnectionContext(
        DbContextOptions<ConnectionContext> options)
        : base(options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }
}