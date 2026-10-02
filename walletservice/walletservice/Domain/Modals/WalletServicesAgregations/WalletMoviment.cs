using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace walletservice.Domain.Modals.WalletServicesAgregations;

[Table ("wallet_movements")]
public class WalletMoviment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public int WalletId { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public WalletMoviment Type{get;set;}
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public DateTime CreatedAt { get; set; }
}