using walletservice.Domain.Modals.Enums;

namespace walletservice.Application.DTOs.Requests;

public class WalletMovimentoRequest
{
    
    public decimal Amount { get; set; }
    
    public string? Description { get; set; }
    
    public string? Reference { get; set; }
    
}