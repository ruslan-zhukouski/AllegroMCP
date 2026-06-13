namespace Server.Models;

public class GetPaymentOperationsResponse
{
    public List<PaymentOperation>? PaymentOperations { get; set; }
    public int? Count { get; set; }
}

public class PaymentOperation
{
    public Wallet? Wallet { get; set; }
}

public class Wallet
{
    public string? PaymentOperator { get; set; }
    public string? Type { get; set; }
    public Balance? Balance { get; set; }
}

public class Balance
{
    public string? Amount { get; set; }
    public string? Currency { get; set; }
}
