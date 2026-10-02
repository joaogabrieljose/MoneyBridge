using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using walletservice.Domain.Modals.Enums;

namespace walletservice.Domain.Modals.WalletServicesAgregations;

[Table ("wallet_movements")]
public class WalletMoviment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long WalletId { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public WalletMovementType Type{get;set;}
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public DateTime CreatedAt { get; set; }

    public WalletMoviment ()
    {
    }

    public WalletMoviment(long id,long walletId,  decimal amount, decimal balanceBefore, decimal balanceAfter,
        WalletMovementType type, string description, string reference, DateTime createdAt)
    {
        Id = id;
        WalletId = walletId;
        Amount = amount;
        BalanceBefore = balanceBefore;
        BalanceAfter = balanceAfter;
        Type = type;
        Description = description;
        Reference = reference;
        CreatedAt = createdAt;
        
    }
}