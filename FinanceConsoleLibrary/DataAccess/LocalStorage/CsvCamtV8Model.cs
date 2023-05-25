using CsvHelper.Configuration.Attributes;

namespace FinanceConsoleLibrary.DataAccess.LocalStorage;
#pragma warning disable CS8618
public class CsvCamtV8Model
{
    [Index(0)] public string OrderAccount { get; set; }
    [Index(1)] public string BookingDate { get; set; }
    [Index(2)] public string ValueDate { get; set; }
    [Index(3)] public string BookingText { get; set; }
    [Index(4)] public string ReasonForPayment { get; set; }
    [Index(5)] public string CreditorId { get; set; }
    [Index(6)] public string MandateReference { get; set; }
    [Index(7)] public string CustomerReference { get; set; }
    [Index(8)] public string CollectorReference { get; set; }
    [Index(9)] public string DirectDebitOriginalAmount { get; set; }
    [Index(10)] public string ReverseDebitReimbursement { get; set; }
    [Index(11)] public string BeneficiaryPayee { get; set; }
    [Index(12)] public string AccountNumberIban { get; set; }
    [Index(13)] public string BicSwift { get; set; }
    [Index(14)] public string Amount { get; set; }
    [Index(15)] public string Currency { get; set; }
    [Index(16)] public string Info { get; set; }
}
#pragma warning restore CS8618