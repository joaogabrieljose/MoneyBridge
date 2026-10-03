using walletservice.Infra.Data;

namespace walletservice.Domain.Modals.WalletServicesAgregations;

public interface IWalletsRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetByCustomerIdAsync(long customerId);
    
    Task<bool> ExistsByCustomerIdAsync(long customerId);
    
}