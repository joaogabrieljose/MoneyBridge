using Microsoft.EntityFrameworkCore.Storage;
using walletservice.Domain.Modals.WalletServicesAgregations;
using walletservice.Infra.Data;

namespace walletservice.Infra.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ConnectionContext _context;
    private IDbContextTransaction? _transaction;
    
    public IWalletsRepository Wallets { get; }
    public IWalletMovimentRepository WalletMovements { get; }
    
    
    public UnitOfWork(ConnectionContext context,  IWalletsRepository wallets, IWalletMovimentRepository walletMovements)
    {
        _context = context;
        wallets = Wallets;
        WalletMovements = walletMovements;
    }
    
    
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction is null) 
            return;
        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_transaction is null)
            return;
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}