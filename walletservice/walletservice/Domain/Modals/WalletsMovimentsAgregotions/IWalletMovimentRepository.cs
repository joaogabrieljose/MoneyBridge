namespace walletservice.Domain.Modals.WalletServicesAgregations;

public interface IWalletMovimentRepository
{
    Task<Wallet?> GetByIdAsync(long id);

    Task<Wallet?> GetByCustomerIdAsync(long customerId);

    Task<List<Wallet>> GetAllAsync();

    Task<bool> ExistsByCustomerIdAsync(long customerId);

    Task AddAsync(Wallet wallet);

    void Update(Wallet wallet);

    void Delete(Wallet wallet);

    Task SaveAsync();
}