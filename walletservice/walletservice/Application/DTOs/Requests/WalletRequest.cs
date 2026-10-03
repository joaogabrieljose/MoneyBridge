using walletservice.Domain.Modals.Enums;

namespace walletservice.Application.DTOs.Requests;

public class WalletRequest
{
    
    public long CustomerId { get; set; }

    public string Currency { get; set; } = "EUR";

    public decimal DailyLimit { get; set; }

    public decimal TransactionLimit { get; set; }
}