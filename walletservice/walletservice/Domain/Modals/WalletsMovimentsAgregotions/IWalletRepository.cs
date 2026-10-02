namespace walletservice.Domain.Modals.WalletServicesAgregations;

public interface IWalletRepository
{
    Task<Wallets?> GetByIdAsync(long id);

    Task<Wallets?> GetByCustomerIdAsync(long customerId);

    Task<List<Wallets>> GetAllAsync();

    Task<bool> ExistsByCustomerIdAsync(long customerId);

    Task AddAsync(Wallets wallet);

    void Update(Wallets wallet);

    void Delete(Wallets wallet);

    Task SaveAsync();
}