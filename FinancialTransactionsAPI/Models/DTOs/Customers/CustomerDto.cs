using FinancialTransactionsAPI.Models.Entities.Transactions;

namespace FinancialTransactionsAPI.Models.DTOs.Customers
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}