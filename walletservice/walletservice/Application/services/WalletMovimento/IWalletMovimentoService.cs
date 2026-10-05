using walletservice.Application.DTOs.Responses;
using walletservice.Domain.Modals.Enums;

namespace walletservice.Application.services.WalletMovimento;

public interface IWalletMovimentoService
{
    Task<WalletMovimentoResponse?> GetByIdAsync(long id);

    Task<List<WalletMovimentoResponse>> GetByWalletIdAsync(
        long walletId);

    Task<List<WalletMovimentoResponse>> GetStatementAsync(
        long walletId);

    Task<List<WalletMovimentoResponse>> GetAllAsync();

    Task<List<WalletMovimentoResponse>> GetByTypeAsync(
        WalletMovementType type);

    Task<List<WalletMovimentoResponse>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate);
}