using System.Text.Json.Serialization;
using FinancialTransactionsAPI.Models.Entities.Transactions;

namespace FinancialTransactionsAPI.Models.Entities.Customers
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}