using FinanceConsoleLibrary.DataAccess.Database.Models;
using FinanceConsoleLibrary.DataAccess.Database.Modules.Instance;
using Spectre.Console;

namespace FinanceConsole.Modules.Static;

public static class UserModule
{
    public static User? Login()
    {
        var loginName = AnsiConsole.Ask<string>("What's your [green]login[/]?");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [green]password[/]?")
                .PromptStyle("red")
                .Secret());

        var userHelper = new UserHelper();
        if (userHelper.Login(loginName, password))
        {
            AnsiConsole.MarkupLine("--- [green]Login successful[/] ---");
            AnsiConsole.WriteLine("Press enter to continue");
            Console.ReadKey();
            if (userHelper.User != null) return userHelper.User;
        }

        AnsiConsole.MarkupLine("--- [red]Login unsuccessful[/] ---");
        AnsiConsole.WriteLine("Press enter to continue");
        Console.ReadKey();
        return null;
    }

    public static User? CreateNewUser()
    {
        var label = AnsiConsole.Ask<string>("What's your [green]Name[/]?");
        var loginName = AnsiConsole.Ask<string>("What should be your [green]login[/]?");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [green]password[/]?")
                .PromptStyle("red")
                .Secret());

        var userHelper = new UserHelper();
        if (userHelper.Create(label, loginName, password))
        {
            AnsiConsole.MarkupLineInterpolated($"--- [green]Created new user for {label} [/] ---");
            AnsiConsole.WriteLine("Press enter to continue");
            Console.ReadKey();
            if (userHelper.User != null) return userHelper.User;
        }

        AnsiConsole.MarkupLine("--- [red]Couldn't create user[/] ---");
        AnsiConsole.WriteLine("Press enter to continue");
        Console.ReadKey();
        return null;
    }
}