using FinanceConsoleLibrary.DataAccess.Database;
using FinanceConsoleLibrary.DataAccess.Database.Models;
using FinanceConsoleLibrary.DataAccess.Database.Modules.Instance;
using FinanceConsoleLibrary.DataAccess.LocalStorage.Modules.Static;
using FinanceConsoleLibrary.Modules.Static;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace FinanceConsole.Modules.Static;

public static class TransactionModule
{
    public static Transaction? Select(Account? account, User? user)
    {
        if (account == null) return null;
        if (user == null) return null;

        var transactionHelper = new TransactionHelper();

        var transaction = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please choose an [green]transaction[/]?")
                .PageSize(25)
                .MoreChoicesText("[grey](Move up and down to reveal more transactions)[/]")
                .AddChoices("New", "Existing", "None")
        );

        switch (transaction)
        {
            case "New":
                return Import(account, user);
            case "None":
                break;
            case "Existing":
                return SelectTransaction(account);
        }

        return null;
    }


    private static Transaction? SelectTransaction(Account account)
    {
        var transactionHelper = new TransactionHelper();
        var transactions = transactionHelper.GetTransactions(account);

        if (transactions.Count == 0)
        {
            AnsiConsole.MarkupLineInterpolated($"No transactions available for {account.Label}");
            return null;
        }

        var transaction = AnsiConsole.Prompt(
            new SelectionPrompt<Transaction>()
                .Title($"Please choose an [green]transaction[/] from {account.Label}:{account.Iban}")
                .PageSize(25)
                .MoreChoicesText("[grey](Move up and down to reveal more transactions)[/]")
                .AddChoices(transactions)
        );

        return transaction;
    }

    private static Transaction? Create(Account account)
    {
        AnsiConsole.MarkupLine("What's the transferred [green]Amount in Euro[/]?");
        AnsiConsole.MarkupLine("exp. 0.7 for 70 Cent or 128.34 for 128 Euro and 34 Cent");
        var amountInCent = AnsiConsole.Ask<decimal>("[green]Amount[/]: ");
        var Target = AnsiConsole.Ask<string>("To which target was the money transferred[green]Amount[/]: ");
        // TODO: Maybe finish?

        return null;
    }

    /// <summary>
    ///     Will always return null!
    /// </summary>
    /// <param name="account">To which account should the CAMT transaction be imported to?</param>
    /// <param name="user">To which user should the CAMT transaction be imported to?</param>
    /// <returns></returns>
    private static Transaction? Import(Account account, User user)
    {
        var filePath = AnsiConsole.Ask<string>("Please provide the full filepath to your CAMTv8 File: ");
        var fileRecords = CamtModule.ReadCsvCamtV8(filePath.Trim());
        var fileTransactions = CamtModule.ReadCamtFileToTransactionsList(fileRecords, user);

        if (fileTransactions != null)
        {
            var storageContext = new StorageContext();
            storageContext.Database.EnsureCreated();
            storageContext.Users.Attach(user);
            storageContext.Accounts.Where(x => x.User.Equals(user)).Load();

            storageContext.Transactions.AddRange(fileTransactions);
            storageContext.SaveChanges();
        }
        else
        {
            LogModule.WriteError("Import of transactions failed, please check the log");
        }

        return null;
    }
}