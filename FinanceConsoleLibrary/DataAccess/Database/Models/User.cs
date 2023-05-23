namespace FinanceConsoleLibrary.DataAccess.Database.Models;

public class User
{
    public byte Id { get; set; }
    public string Label { get; set; }
    public string LoginName { get; set; }
    public string PasswordHash { get; set; }
}