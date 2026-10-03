using walletservice.Domain.Modals.Enums;

namespace walletservice.Application.DTOs.Responses;

public class WalletMovimentoResponse
{
    public long Id { get; set; }
    
    public long WalletId { get; set; }
    
    public decimal Amount { get; set; }
    
    public decimal BalanceBefore { get; set; }
    
    public decimal BalanceAfter { get; set; }
    
    public WalletMovementType Type{get;set;}
    
    public string? Description { get; set; }
    
    public string? Reference { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
}