using FinanceConsoleLibrary.DataAccess.Database.Models;
using FinanceConsoleLibrary.Modules.Static;
using Microsoft.EntityFrameworkCore;

namespace FinanceConsoleLibrary.DataAccess.Database.Modules.Instance;

public class AccountHelper
{
    public Account? Account { get; private set; }

    public ICollection<Account>? GetAccounts(User user)
    {
        var storageContext = new StorageContext();
        storageContext.Database.EnsureCreated();

        return storageContext.Accounts.Where(x => x.UserId.Equals(user.Id)).ToList();
    }

    public bool Create(User user, string label, string iban)
    {
        var storageContext = new StorageContext();
        storageContext.Database.EnsureCreated();

        storageContext.Users.Attach(user);
        var tempAccount = storageContext.Accounts.Add(new Account
        {
            Label = label,
            Iban = iban,
            User = user,
            UserId = user.Id
        });

        try
        {
            storageContext.SaveChanges();
            Account = tempAccount.Entity;
            return true;
        }
        catch (DbUpdateException e)
        {
            LogModule.WriteError("Could not save account!", e);
            return false;
        }
    }
}