using walletservice.Domain.Modals.Enums;

namespace walletservice.Application.DTOs.Responses;

public class WalletResponse
{
    public  long Id { get; set; }                                 
    
    public long CustomerId { get; set; }
    
    public decimal Balance { get; set; }
    
    public string Currency { get; set; }
    
    public WalletStatus Status { get; set; }
    
    public decimal DailyLimit { get; set; }
    
    public decimal TransactionLimit { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? BlockedAt { get; set; }
    
    
}