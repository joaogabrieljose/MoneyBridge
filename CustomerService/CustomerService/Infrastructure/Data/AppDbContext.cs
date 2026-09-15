using CustomerService.Domain.Modals;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }
    
    public DbSet<Customer>Customers { get; set; }
    

}