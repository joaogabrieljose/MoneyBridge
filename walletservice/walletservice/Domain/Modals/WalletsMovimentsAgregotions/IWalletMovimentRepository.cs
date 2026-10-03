using walletservice.Infra.Data;

namespace walletservice.Domain.Modals.WalletServicesAgregations;

public interface IWalletMovimentRepository : IGenericRepository<WalletMoviment>
{
   Task<List<WalletMoviment>> GetAllByWalletIdAsync(long walletId);
}