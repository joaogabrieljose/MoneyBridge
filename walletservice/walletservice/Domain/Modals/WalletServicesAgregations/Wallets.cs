using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using walletservice.Domain.Modals.Enums;

namespace walletservice.Domain.Modals.WalletServicesAgregations;

public class Wallets
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    public Wallets(int id, int customerId,  decimal balance, string currency, WalletStatus status,  decimal transactionLimit, DateTime createdAt, DateTime? updatedAt, DateTime? blockedAt )
    {
        Id = id;
        CustomerId = customerId;
        Balance = balance;
        Currency = currency;
        Status = status;
        TransactionLimit = transactionLimit;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        BlockedAt = blockedAt;
    }
}