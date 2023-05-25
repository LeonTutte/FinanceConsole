using FinanceConsoleLibrary.DataAccess.Database.Models;
using FinanceConsoleLibrary.DataAccess.Database.Modules.Instance;
using Spectre.Console;

namespace FinanceConsole.Modules.Static;

public static class AccountModule
{
    public static Account? Select(User? user)
    {
        if (user == null) return null;

        var accountHelper = new AccountHelper();
        var userAccountStringList = new List<string>();

        if (accountHelper.GetAccounts(user) != null)
            foreach (var item in accountHelper.GetAccounts(user))
                userAccountStringList.Add(item.Label);

        var account = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please choose an [green]account[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more accounts)[/]")
                .AddChoices("New", "None")
                .AddChoices(userAccountStringList)
        );

        switch (account)
        {
            case "New":
                Create(user);
                break;
            case "None":
                break;
            default:
                var result = accountHelper.GetAccounts(user)?
                    .Where(x => x.Label.Equals(account)).SingleOrDefault(x => x.UserId.Equals(user.Id));
                return result;
        }

        return null;
    }

    private static Account? Create(User user)
    {
        var label = AnsiConsole.Ask<string>("What's the [green]account name[/]?");
        var iban = AnsiConsole.Ask<string>("What's the [green]IBAN[/]?");

        var accountHelper = new AccountHelper();
        if (accountHelper.Create(user, label, iban))
        {
            AnsiConsole.MarkupLineInterpolated($"--- [green]Created new account for {user.Label} [/] ---");
            AnsiConsole.WriteLine("Press enter to continue");
            Console.ReadKey();
            if (accountHelper.Account != null) return accountHelper.Account;
        }

        AnsiConsole.MarkupLine("--- [red]Couldn't create account[/] ---");
        AnsiConsole.WriteLine("Press enter to continue");
        Console.ReadKey();
        return null;
    }
}