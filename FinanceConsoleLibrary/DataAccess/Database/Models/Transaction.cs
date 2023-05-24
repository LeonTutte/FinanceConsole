#pragma warning disable CS8618

namespace FinanceConsoleLibrary.DataAccess.Database.Models;

public class Transaction
{
    public ulong Id { get; set; }
    public int AmountInCent { get; set; }
    public string Target { get; set; }
    public string TargetIban { get; set; }
    public string Description { get; set; }
    public string Reference { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime ValueDate { get; set; }
    public ushort BaseAccountId { get; set; }

    public virtual Account BaseAccount { get; set; }
}