using FinanceConsoleLibrary.DataAccess.Database.Models;
using FinanceConsoleLibrary.Modules.Static;
using Microsoft.EntityFrameworkCore;

namespace FinanceConsoleLibrary.DataAccess.Database.Modules.Instance;

public class TransactionHelper
{
    public Transaction? Transaction { get; private set; }

    public ICollection<Transaction> GetTransactions(Account? account)
    {
        if (account == null) return new List<Transaction>();

        var storageContext = new StorageContext();
        storageContext.Database.EnsureCreated();

        ICollection<Transaction>? transactions = new List<Transaction>();

        var tempTransactions = storageContext.Transactions.Where(x => x.BaseAccountId.Equals(account.Id))
            .Include(x => x.BaseAccount).ToList();
        if (tempTransactions != null)
            foreach (var tempTransaction in tempTransactions)
                transactions.Add(tempTransaction);

        return transactions;
    }

    public bool Create(Account account, int amountInCent, string target, string targetIban, DateTime bookingDate,
        DateTime valueDate, string? description = null, string? reference = null)
    {
        var storageContext = new StorageContext();
        storageContext.Database.EnsureCreated();
        storageContext.Accounts.Attach(account);

        var tempTransaction = storageContext.Transactions.Add(new Transaction
        {
            AmountInCent = amountInCent,
            Target = target,
            TargetIban = targetIban,
            BookingDate = bookingDate,
            ValueDate = valueDate,
            BaseAccount = account,
            BaseAccountId = account.Id
        });

        if (description != null) tempTransaction.Entity.Description = description;

        if (reference != null) tempTransaction.Entity.Reference = reference;

        try
        {
            storageContext.SaveChanges();
            Transaction = tempTransaction.Entity;
            return true;
        }
        catch (DbUpdateException e)
        {
            LogModule.WriteError("Could not save transaction!", e);
            return false;
        }
    }
}