#pragma warning disable CS8618

namespace FinanceConsoleLibrary.DataAccess.Database.Models;

public class Account
{
    public ushort Id { get; set; }
    public string Label { get; set; }
    public string? Iban { get; set; }

    public ushort UserId { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; }
}