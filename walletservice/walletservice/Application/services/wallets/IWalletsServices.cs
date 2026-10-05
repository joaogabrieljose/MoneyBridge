using walletservice.Application.DTOs.Requests;
using walletservice.Application.DTOs.Responses;
using walletservice.Domain.Modals.WalletServicesAgregations;

namespace walletservice.Application.services.wallets;

public interface IWalletsServices
{
    Task<WalletResponse> CreateWalletAsync(WalletRequest wallet);

    Task<WalletResponse?> GetByIdAsync(long id);

    Task<WalletResponse?> GetByCustomerIdAsync(long customerId);

    Task<List<WalletResponse>> GetAllAsync();

    Task<decimal> GetBalanceAsync(long walletId);

    Task<WalletResponse> BlockAsync(long walletId);

    Task<WalletResponse> UnblockAsync(long walletId);

    Task<WalletResponse> UpdateLimitsAsync(long walletId, WalletRequest request);

    Task<List<WalletMovimentoResponse>> GetStatementAsync(long walletId);
}
