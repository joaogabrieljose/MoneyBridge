using walletservice.Application.DTOs.Requests;
using walletservice.Application.DTOs.Responses;
using walletservice.Domain.Modals.Enums;
using walletservice.Domain.Modals.WalletServicesAgregations;
using walletservice.Infra.UnitOfWork;

namespace walletservice.Application.services.wallets;

public class WalletService : IWalletsServices
{
    private readonly IUnitOfWork _unitOfWork;

    public WalletService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // CREATE WALLET
    public async Task<WalletResponse> CreateWalletAsync(
        WalletRequest request)
    {
        var walletExists = await _unitOfWork.Wallets.ExistsByCustomerIdAsync(request.CustomerId);

        if (walletExists)
        {
            throw new InvalidOperationException(
                "Este cliente já possui uma carteira.");
        }

        if (request.DailyLimit <= 0)
        {
            throw new ArgumentException("O limite diário deve ser superior a zero.");
        }

        if (request.TransactionLimit <= 0)
        {
            throw new ArgumentException("O limite por transação deve ser superior a zero.");
        }

        if (request.TransactionLimit > request.DailyLimit)
        {
            throw new InvalidOperationException("O limite por transação não pode ser superior ao limite diário.");
        }

        var wallet = new Wallet
        {
            CustomerId = request.CustomerId, Balance = 0, Currency = request.Currency, Status = WalletStatus.Active,
            DailyLimit = request.DailyLimit, TransactionLimit = request.TransactionLimit,
            CreatedAt = DateTime.UtcNow, UpdatedAt = null, BlockedAt = null
        };

        await _unitOfWork.Wallets.AddAsync(wallet);

        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(wallet);
    }


    // GET BY ID
    public async Task<WalletResponse?> GetByIdAsync(long id)
    {
        var wallet = await _unitOfWork.Wallets.GetByIdAsync(id);

        if (wallet is null)
        {
            return null;
        }

        return MapToResponse(wallet);
    }


    // GET BY CUSTOMER
    public async Task<WalletResponse?> GetByCustomerIdAsync(
        long customerId)
    {
        var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId);
        if (wallet is null)
        {
            return null;
        }
        
        return MapToResponse(wallet);
    }


    // GET ALL
    public async Task<List<WalletResponse>> GetAllAsync()
    {
        var wallets = await _unitOfWork.Wallets.GetAllAsync();
        return wallets.Select(MapToResponse).ToList();
    }


    // CONSULTAR SALDO
    public async Task<decimal> GetBalanceAsync(long walletId)
    {
        var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);

        if (wallet is null)
        {
            throw new KeyNotFoundException($"Carteira com ID {walletId} não encontrada.");
        }
        return wallet.Balance;
    }


    // BLOQUEAR CARTEIRA
    public async Task<WalletResponse> BlockAsync(long walletId)
    {
        var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);

        if (wallet is null)
        {
            throw new KeyNotFoundException($"Carteira com ID {walletId} não encontrada.");
        }

        if (wallet.Status == WalletStatus.Blocked)
        {
            throw new InvalidOperationException("A carteira já está bloqueada.");
        }

        wallet.Status = WalletStatus.Blocked;
        wallet.BlockedAt = DateTime.UtcNow;
        wallet.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Wallets.Update(wallet);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(wallet);
    }


    // DESBLOQUEAR CARTEIRA
    public async Task<WalletResponse> UnblockAsync(long walletId)
    {
        var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);

        if (wallet is null)
        {
            throw new KeyNotFoundException($"Carteira com ID {walletId} não encontrada.");
        }

        if (wallet.Status != WalletStatus.Blocked)
        {
            throw new InvalidOperationException("A carteira não está bloqueada.");
        }

        wallet.Status = WalletStatus.Active;
        wallet.BlockedAt = null;
        wallet.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Wallets.Update(wallet);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(wallet);
    }

    public async Task<WalletResponse> UpdateLimitsAsync(long walletId, WalletRequest request)
    {
        var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);

        if (wallet is null)
        {
            throw new KeyNotFoundException($"Carteira com ID {walletId} não encontrada.");
        }

        if (request.DailyLimit <= 0)
        {
            throw new ArgumentException("O limite diário deve ser superior a zero.");
        }

        if (request.TransactionLimit <= 0)
        {
            throw new ArgumentException("O limite por transação deve ser superior a zero.");
        }

        if (request.TransactionLimit > request.DailyLimit)
        {
            throw new InvalidOperationException("O limite por transação não pode ser superior ao limite diário.");
        }

        wallet.DailyLimit = request.DailyLimit;
        wallet.TransactionLimit = request.TransactionLimit;
        wallet.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Wallets.Update(wallet);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(wallet);
    }

    public async Task<List<WalletMovimentoResponse>> GetStatementAsync(long walletId)
    {
        var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);

        if (wallet is null)
        {
            throw new KeyNotFoundException($"Carteira com ID {walletId} não encontrada.");
        }

        var movements = await _unitOfWork.WalletMovements.GetAllByWalletIdAsync(walletId);

        return movements.Select(MapMovementToResponse).ToList();
    }


    // ATUALIZAR LIMITES
    public async Task<WalletResponse> UpdateLimitsAsync(long walletId, decimal dailyLimit, decimal transactionLimit)
    {
        var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);

        if (wallet is null)
        {
            throw new KeyNotFoundException($"Carteira com ID {walletId} não encontrada.");
        }

        if (dailyLimit <= 0)
        {
            throw new ArgumentException("O limite diário deve ser superior a zero.");
        }

        if (transactionLimit <= 0)
        {
            throw new ArgumentException("O limite por transação deve ser superior a zero.");
        }

        if (transactionLimit > dailyLimit)
        {
            throw new InvalidOperationException("O limite por transação não pode ser superior ao limite diário.");
        }

        wallet.DailyLimit = dailyLimit;
        wallet.TransactionLimit = transactionLimit;
        wallet.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Wallets.Update(wallet);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(wallet);
    }


    // CREDITAR SALDO
    public async Task<WalletResponse> CreditAsync(
        long walletId,
        WalletMovimentoRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentException(
                "O valor deve ser superior a zero.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var wallet =
                await _unitOfWork.Wallets.GetByIdAsync(walletId);

            if (wallet is null)
            {
                throw new KeyNotFoundException(
                    $"Carteira com ID {walletId} não encontrada.");
            }

            if (wallet.Status != WalletStatus.Active)
            {
                throw new InvalidOperationException(
                    "A carteira não está ativa.");
            }

            var balanceBefore = wallet.Balance;

            wallet.Balance += request.Amount;
            wallet.UpdatedAt = DateTime.UtcNow;

            var movement = new WalletMoviment
            {
                WalletId = wallet.Id,

                Amount = request.Amount,

                BalanceBefore = balanceBefore,

                BalanceAfter = wallet.Balance,

                Type = WalletMovementType.Credit,

                Description = request.Description,

                Reference = request.Reference,

                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Wallets.Update(wallet);

            await _unitOfWork.WalletMovements
                .AddAsync(movement);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();

            return MapToResponse(wallet);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();

            throw;
        }
    }


    // DEBITAR SALDO
    public async Task<WalletResponse> DebitAsync(
        long walletId,
        WalletMovimentoRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentException(
                "O valor deve ser superior a zero.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var wallet =
                await _unitOfWork.Wallets.GetByIdAsync(walletId);

            if (wallet is null)
            {
                throw new KeyNotFoundException(
                    $"Carteira com ID {walletId} não encontrada.");
            }

            if (wallet.Status != WalletStatus.Active)
            {
                throw new InvalidOperationException(
                    "A carteira não está ativa.");
            }

            if (wallet.Balance < request.Amount)
            {
                throw new InvalidOperationException(
                    "Saldo insuficiente.");
            }

            if (request.Amount > wallet.TransactionLimit)
            {
                throw new InvalidOperationException(
                    "O valor ultrapassa o limite por transação.");
            }

            var balanceBefore = wallet.Balance;

            wallet.Balance -= request.Amount;
            wallet.UpdatedAt = DateTime.UtcNow;

            var movement = new WalletMoviment
            {
                WalletId = wallet.Id,

                Amount = request.Amount,

                BalanceBefore = balanceBefore,

                BalanceAfter = wallet.Balance,

                Type = WalletMovementType.Debit,

                Description = request.Description,

                Reference = request.Reference,

                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Wallets.Update(wallet);

            await _unitOfWork.WalletMovements
                .AddAsync(movement);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();

            return MapToResponse(wallet);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();

            throw;
        }
    }


    // MAPPING ENTITY -> RESPONSE
    private static WalletResponse MapToResponse(Wallet wallet)
    {
        return new WalletResponse
        { Id = wallet.Id,
            CustomerId = wallet.CustomerId, Balance = wallet.Balance, Currency = wallet.Currency, Status = wallet.Status,
            DailyLimit = wallet.DailyLimit, TransactionLimit = wallet.TransactionLimit, CreatedAt = wallet.CreatedAt,
            UpdatedAt = wallet.UpdatedAt, BlockedAt = wallet.BlockedAt
        };
    }
    private static WalletMovimentoResponse MapMovementToResponse(
        WalletMoviment movement)
    {
        return new WalletMovimentoResponse
        { Id = movement.Id, WalletId = movement.WalletId, Amount = movement.Amount, BalanceBefore = movement.BalanceBefore, 
            BalanceAfter = movement.BalanceAfter, 
            Type = movement.Type, Description = movement.Description, Reference = movement.Reference, CreatedAt = movement.CreatedAt
        };
    }
}