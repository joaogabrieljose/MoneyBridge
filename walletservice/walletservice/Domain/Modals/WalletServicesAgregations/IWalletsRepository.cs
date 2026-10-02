namespace walletservice.Domain.Modals.WalletServicesAgregations;

public interface IWalletsRepository
{
    Task<Wallets?> GetByIdAsync(long id);

    Task<Wallets?> GetByCustomerIdAsync(long customerId);

    Task<List<Wallets>> GetAllAsync();

    Task AddAsync(Wallets wallet);

    void Update(Wallets wallet);

    void Delete(Wallets wallet);

    Task<bool> ExistsByCustomerIdAsync(long customerId);

    Task SaveAsync();
}