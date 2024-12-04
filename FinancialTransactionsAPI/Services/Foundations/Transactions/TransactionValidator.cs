using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FinancialTransactionsAPI.Models.DTOs.Transactions;
using FinancialTransactionsAPI.Models.Entities.Transactions;

namespace FinancialTransactionsAPI.Services.Foundations.Transactions
{
    public class TransactionValidator
    {
        public void ValidateTransactionData(CreateTransactionDto createTransactionDto)
        {
            if (createTransactionDto == null)
            {
                throw new ValidationException("Transaction data is required.");
            }

            if (createTransactionDto.CustomerId == Guid.Empty)
            {
                throw new ValidationException("CustomerId cannot be empty.");
            }

            if (createTransactionDto.Amount <= 0)
            {
                throw new ValidationException("Transaction amount must be greater than zero.");
            }

            if (string.IsNullOrEmpty(createTransactionDto.Description))
            {
                throw new ValidationException("Transaction description cannot be empty.");
            }

            if (!Enum.IsDefined(typeof(TransactionType), createTransactionDto.TransactionType))
            {
                throw new ValidationException($"Invalid TransactionType value: {createTransactionDto.TransactionType}");
            }

            if (!Enum.IsDefined(typeof(Status), createTransactionDto.Status))
            {
                throw new ValidationException($"Invalid TransactionStatus value: {createTransactionDto.Status}");
            }
        }
        
        public void ValidateUpdateTransactionFilter(UpdateTransactionDto updateTransactionDto)
        {

            if (!string.IsNullOrEmpty(updateTransactionDto.TransactionType.ToString()))
            {
                if (!Enum.IsDefined(typeof(TransactionType), updateTransactionDto.TransactionType))
                {
                    throw new ValidationException($"Invalid TransactionType value: {updateTransactionDto.TransactionType}");
                }
            }

            if (!string.IsNullOrEmpty(updateTransactionDto.Status.ToString()))
            {
                if (!Enum.IsDefined(typeof(Status), updateTransactionDto.Status))
                {
                    throw new ValidationException($"Invalid TransactionStatus value: {updateTransactionDto.Status}");
                }
            }
        }

        public void ValidateTransactionFilter(TransactionFilter filter)
        {
            if (filter.PageNumber.HasValue && filter.PageNumber.Value <= 0)
            {
                throw new ArgumentException("Page number must be greater than 0.");
            }

            if (filter.PageSize.HasValue && (filter.PageSize.Value <= 0 || filter.PageSize.Value > 100))
            {
                throw new ArgumentException("Page size must be between 1 and 100.");
            }

            // if (!Enum.IsDefined(typeof(TransactionType), filter.TransactionType))
            // {
            //     throw new ValidationException($"Invalid TransactionType value: {filter.TransactionType}");
            // }

            if (!string.IsNullOrEmpty(filter.TransactionType.ToString()))
            {
                if (!Enum.IsDefined(typeof(TransactionType), filter.TransactionType))
                {
                    throw new ValidationException($"Invalid TransactionType value: {filter.TransactionType}");
                }
            }

            if (!string.IsNullOrEmpty(filter.TransactionStatus.ToString()))
            {
                if (!Enum.IsDefined(typeof(Status), filter.TransactionStatus))
                {
                    throw new ValidationException($"Invalid TransactionStatus value: {filter.TransactionStatus}");
                }
            }

            if (!string.IsNullOrEmpty(filter.TransactionAmount))
            {
                if (!decimal.TryParse(filter.TransactionAmount, out var transactionAmount))
                {
                    throw new ArgumentException($"Invalid TransactionAmount value: {filter.TransactionAmount}");
                }
            }

            if (!string.IsNullOrEmpty(filter.CustomerEmail))
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(filter.CustomerEmail))
                {
                    throw new ArgumentException($"Invalid CustomerEmail format: {filter.CustomerEmail}");
                }
            }
        }
    }
}