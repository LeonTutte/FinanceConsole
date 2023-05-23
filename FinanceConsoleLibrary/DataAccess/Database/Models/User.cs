namespace FinanceConsoleLibrary.DataAccess.Database.Models;

public class User
{
    public ushort Id { get; set; }
    public string Label { get; set; }
    public string LoginName { get; set; }
    public string PasswordHash { get; set; }

    public virtual ICollection<Account> Accounts { get; set; }
}