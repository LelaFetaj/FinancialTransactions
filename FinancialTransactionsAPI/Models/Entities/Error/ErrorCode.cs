namespace FinancialTransactionsAPI.Models.Entities.Error
{
    public enum ErrorCode
    {
        InvalidInput = 1001,
        InternalError = 1002,
        NotFound = 1003,
        Unauthorized = 1004,
        TransactionCreationError = 1005
    }
}