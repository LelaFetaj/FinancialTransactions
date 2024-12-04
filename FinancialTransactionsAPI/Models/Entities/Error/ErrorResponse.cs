namespace FinancialTransactionsAPI.Models.Entities.Error
{
    public class ErrorResponse
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}