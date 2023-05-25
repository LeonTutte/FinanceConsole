using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FinanceConsoleLibrary.DataAccess.Database;
using FinanceConsoleLibrary.DataAccess.Database.Models;
using FinanceConsoleLibrary.Modules.Static;

namespace FinanceConsoleLibrary.DataAccess.LocalStorage.Modules.Static;

public static class CamtModule
{
    public static List<CsvCamtV8Model> ReadCsvCamtV8(string inputFilePath, char delimiter = ';', bool hasHeader = true)
    {
        if (inputFilePath == null) throw new ArgumentNullException(nameof(inputFilePath));
        if (!File.Exists(inputFilePath)) throw new FileNotFoundException(nameof(inputFilePath));
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = hasHeader,
            Delimiter = delimiter.ToString()
        };
        using (var reader = new StreamReader(inputFilePath))
        using (var csv = new CsvReader(reader, csvConfiguration))
        {
            var records = csv.GetRecords<CsvCamtV8Model>();
            return records.ToList();
        }
    }

    public static ICollection<Transaction>? ReadCamtFileToTransactionsList(List<CsvCamtV8Model> records, User user)
    {
        var storageContext = new StorageContext();
        storageContext.Database.EnsureCreated();
        storageContext.Users.Attach(user);

        ICollection<Account> accounts = storageContext.Accounts.Where(x => x.UserId.Equals(user.Id)).ToList();
        var transactions = new List<Transaction>();

        if (accounts.Count == 0)
        {
            LogModule.WriteError("Cant import without accounts, since there will be no matchup available!");
            return null;
        }

        var missing = 0;

        foreach (var camtRecord in records)
        {
            if (accounts.SingleOrDefault(x => x.Iban != null && x.Iban.Equals(camtRecord.OrderAccount.Trim())) == null)
            {
                missing++;
                break;
            }

            ;

            var tempTransaction = new Transaction
            {
                AmountInCent = (int)(double.Parse(camtRecord.Amount) * 100),
                Target = string.Join(" ", camtRecord.BeneficiaryPayee.Trim().Split(
                        new char[0], StringSplitOptions.RemoveEmptyEntries)
                    .ToList().Select(x => x.Trim())),
                TargetIban = camtRecord.AccountNumberIban.Trim(),
                Description = camtRecord.BookingText.Trim(),
                Reference = camtRecord.MandateReference.Trim(),
                BaseAccount = accounts.SingleOrDefault(x => x.Iban.Equals(camtRecord.OrderAccount.Trim()))
            };

            if (DateTime.TryParse(camtRecord.ValueDate, out var vDate)) tempTransaction.ValueDate = vDate;
            if (DateTime.TryParse(camtRecord.BookingDate, out var bDate)) tempTransaction.BookingDate = bDate;

            transactions.Add(tempTransaction);
        }

        if (missing > 0) LogModule.WriteError($"Could not find matching accounts for {missing} records");

        return transactions;
    }
}