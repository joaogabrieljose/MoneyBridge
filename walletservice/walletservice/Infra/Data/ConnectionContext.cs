using Microsoft.EntityFrameworkCore;
using walletservice.Domain.Modals.WalletServicesAgregations;

namespace walletservice.Infra.Data;

public class ConnectionContext : DbContext
{
    public ConnectionContext(
        DbContextOptions<ConnectionContext> options) 
        : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletMoviment> WalletMoviments { get; set; }
}