using walletservice.Application.DTOs.Responses;
using walletservice.Domain.Modals.Enums;
using walletservice.Domain.Modals.WalletServicesAgregations;

namespace walletservice.Application.services.WalletMovimento;

public class WalletMovimentoService : IWalletMovimentoService
{
    private readonly IWalletMovimentRepository _walletMovimentRepository;

    public WalletMovimentoService(IWalletMovimentRepository walletMovimentRepository)
    {
        _walletMovimentRepository = walletMovimentRepository;
    }
    

    public async Task<WalletMovimentoResponse?> GetByIdAsync(long id)
    {
        var movimento = await _walletMovimentRepository.GetByIdAsync(id);
        if (movimento is null) return null;
        return MapToResponse(movimento);
    }

    

    public async Task<List<WalletMovimentoResponse>> GetByWalletIdAsync(long walletId)
    {
        var movimentos = await _walletMovimentRepository.GetAllByWalletIdAsync(walletId);
        return movimentos.Select(MapToResponse).ToList();
    }

    

    public async Task<List<WalletMovimentoResponse>> GetStatementAsync(long walletId)
    {
        var movimentos = await _walletMovimentRepository.GetAllByWalletIdAsync(walletId);
        return movimentos.OrderByDescending(x => x.CreatedAt).Select(MapToResponse).ToList();
    }



    public async Task<List<WalletMovimentoResponse>> GetAllAsync()
    {
        var movimentos = await _walletMovimentRepository.GetAllAsync();
        return movimentos.OrderByDescending(x => x.CreatedAt).Select(MapToResponse).ToList();
    }
    

    public async Task<List<WalletMovimentoResponse>> GetByTypeAsync(WalletMovementType type)
    {
        var movimentos = await _walletMovimentRepository.GetAllAsync();
        return movimentos
            .Where(x => x.Type == type).OrderByDescending(x => x.CreatedAt).Select(MapToResponse).ToList();
    }
    

    public async Task<List<WalletMovimentoResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate) throw new ArgumentException("A data inicial não pode ser superior à data final.");

        var movimentos = await _walletMovimentRepository.GetAllAsync();

        return movimentos.Where(x =>
                x.CreatedAt >= startDate &&
                x.CreatedAt <= endDate)
            .OrderByDescending(x => x.CreatedAt).Select(MapToResponse).ToList();
    }
    

    private static WalletMovimentoResponse MapToResponse(WalletMoviment movimento)
    {
        return new WalletMovimentoResponse
        {
            Id = movimento.Id,
            WalletId = movimento.WalletId,
            Amount = movimento.Amount,
            BalanceBefore = movimento.BalanceBefore,
            BalanceAfter = movimento.BalanceAfter,
            Type = movimento.Type,
            Description = movimento.Description,
            Reference = movimento.Reference,
            CreatedAt = movimento.CreatedAt
        };
    }
}