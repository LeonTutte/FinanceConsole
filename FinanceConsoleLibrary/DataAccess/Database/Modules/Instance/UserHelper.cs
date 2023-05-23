using System.Transactions;
using FinanceConsoleLibrary.DataAccess.Database.Models;
using FinanceConsoleLibrary.Modules.Static;

namespace FinanceConsoleLibrary.DataAccess.Database.Modules.Instance;

public class UserHelper
{
    public User? User { get; private set; }

    /// <summary>
    ///     Checks if a user exists in the storage and tries to authenticate
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="password"></param>
    /// <returns>false if user doesn't exist, otherwise the function returns true and hands over the record to the User field</returns>
    public bool Login(string loginName, string password)
    {
        var passwordHash = CryptoModule.CreateSha512(password);
        var storageContext = new StorageContext();

        storageContext.Database.EnsureCreated();
        var tempUser = storageContext.Users.SingleOrDefault(x => x.LoginName.Equals(loginName.Trim()));

        if (tempUser == null) return false;

        if (tempUser.PasswordHash != passwordHash) return false;

        User = tempUser;
        return true;
    }

    public bool Create(string label, string loginName, string password)
    {
        var passwordHash = CryptoModule.CreateSha512(password);
        var storageContext = new StorageContext();

        storageContext.Database.EnsureCreated();
        var tempUser = storageContext.Users.Add(new User
        {
            Label = label,
            LoginName = loginName,
            PasswordHash = password
        });

        try
        {
            storageContext.SaveChanges();
            User = tempUser.Entity;
            return true;
        }
        catch (TransactionException e)
        {
            LogModule.WriteError("Could not save user!");
            LogModule.WriteError(e.Message);
            return false;
        }
    }
}