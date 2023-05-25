using FinanceConsole.Modules.Static;
using FinanceConsoleLibrary.DataAccess.Database.Models;
using Spectre.Console;

namespace FinanceConsole.Modules.Instance;

public class MainMenu
{
    private string? _lastConsoleInput;
    private ConsoleKey? _lastConsoleKey;
    private User? _selectedUser;

    public void OpenMenu()
    {
        while (_lastConsoleKey != ConsoleKey.Q)
        {
            Thread.Sleep(1000);
            AnsiConsole.Clear();
            CreateSelectedStatusTable();
            AnsiConsole.Markup("");
            AnsiConsole.MarkupLine("--- [green]Please choose an action[/] ---");
            if (_selectedUser == null)
            {
                AnsiConsole.WriteLine("A: Create new user");
                AnsiConsole.WriteLine("B: Login as user");
            }

            if (_selectedUser != null)
            {
                AnsiConsole.WriteLine("C: Select account");
                AnsiConsole.WriteLine("D: Select transaction");
                AnsiConsole.WriteLine("E: End Session / Logout");
            }

            AnsiConsole.MarkupLine("--- [grey]Quit by pressing Q[/] ---");

            UserInputPrompt(true);
            switch (_lastConsoleKey)
            {
                case ConsoleKey.Q:
                    AnsiConsole.Clear();
                    break;
                case ConsoleKey.A:
                    _selectedUser = UserModule.CreateNewUser();
                    break;
                case ConsoleKey.B:
                    _selectedUser = UserModule.Login();
                    break;
                case ConsoleKey.C:
                    _selectedAccount = AccountModule.Select(_selectedUser);
                    break;
                case ConsoleKey.D:
                    _selectedTransaction = TransactionModule.Select(_selectedAccount, _selectedUser);
                    break;
                case ConsoleKey.E:
                    _selectedUser = null;
                    break;
                default:
                    AnsiConsole.Markup("[red]Invalid Input![/]");
                    break;
            }
        }
    }

    private void UserInputPrompt(bool key = false)
    {
        AnsiConsole.Markup("[blue]//:[/] ");
        if (key)
        {
            _lastConsoleKey = Console.ReadKey().Key;
            AnsiConsole.WriteLine();
        }
        else
        {
            _lastConsoleInput = Console.ReadLine();
            AnsiConsole.WriteLine();
        }
    }

    private void CreateSelectedStatusTable()
    {
        if (_selectedUser == null && _selectedAccount == null && _selectedTransaction == null) return;

        var table = new Table();

        table.AddColumn("User");
        table.AddColumn("Account");
        table.AddColumn("Transaction");

        var user = _selectedUser != null ? _selectedUser.Label : string.Empty;
        var account = _selectedAccount != null ? _selectedAccount.Label : string.Empty;
        var transaction = _selectedTransaction != null
            ? $"{_selectedTransaction.Id}:{_selectedTransaction.AmountInCent / 100} -> {_selectedTransaction.Target}"
            : string.Empty;

        table.AddRow(user, account, transaction);
        table.Border(TableBorder.Rounded);
        table.Columns[2].NoWrap();

        AnsiConsole.Write(table);
        AnsiConsole.Markup("");
    }
#pragma warning disable CS0649
    private Account? _selectedAccount;
    private Transaction? _selectedTransaction;
#pragma warning restore CS0649
}