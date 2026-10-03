using walletservice.Domain.Modals.WalletServicesAgregations;

namespace walletservice.Infra.UnitOfWork;

public interface IUnitOfWork
{
    
    IWalletsRepository Wallets { get; }

    IWalletMovimentRepository WalletMovements { get; }

    Task<int> SaveChangesAsync();

    Task BeginTransactionAsync();

    Task CommitAsync();

    Task RollbackAsync();
}