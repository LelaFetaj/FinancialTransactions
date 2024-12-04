using FinancialTransactionsAPI.Models.DTOs.Transactions;
using FinancialTransactionsAPI.Models.Entities;
using FinancialTransactionsAPI.Models.Entities.Error;
using FinancialTransactionsAPI.Models.Entities.Transactions;
using FinancialTransactionsAPI.Services.Foundations.Transactions;
using FinancialTransactionsAPI.Services.Orchestrations;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsAPI.Controllers.Transactions
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionOrchestrationService transactionOrchestrationService;
        private readonly ILogger<TransactionController> logger;

        public TransactionController(ITransactionOrchestrationService transactionOrchestrationService, ILogger<TransactionController> logger)
        {
            this.transactionOrchestrationService = transactionOrchestrationService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransactionAsync([FromBody] CreateTransactionDto createTransactionDto)
        {
            try
            {
                var transactionDto = await transactionOrchestrationService.CreateTransactionAsync(createTransactionDto);
                return CreatedAtAction(nameof(GetTransactionById), new { id = transactionDto.TransactionId }, transactionDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = ErrorCode.InvalidInput,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during transaction creation.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransactionById(Guid id)
        {
            try
            {
                var transaction = await transactionOrchestrationService.GetTransactionByIdAsync(id);

                if (transaction == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorCode = ErrorCode.NotFound,
                        ErrorMessage = $"Transaction with ID {id} not found."
                    });
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching the transaction.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetFilteredTransactions([FromQuery] TransactionFilter filter)
        {
            try
            {
                var transactions = await transactionOrchestrationService.GetFilteredTransactionsAsync(filter);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching filtered transactions.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ModifyTransactionAsync(UpdateTransactionDto updateTransactionDto)
        {
            try
            {
                var transactionDto = await transactionOrchestrationService.ModifyTransactionAsync(updateTransactionDto);

                if (transactionDto == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorCode = ErrorCode.NotFound,
                        ErrorMessage = "Transaction not found."
                    });
                }

                return Ok(transactionDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while modifying the transaction.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> RemoveTransactionAsync(Guid transactionId)
        {
            try
            {
                await transactionOrchestrationService.RemoveTransactionAsync(transactionId);
        
                return Ok(new { message = "Transaction successfully deleted." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the transaction.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpDelete("softDeletion")]
        public async Task<IActionResult> SoftDeletionTransactionAsync(Guid transactionId)
        {
            try
            {
                await transactionOrchestrationService.SoftDeleteTransactionAsync(transactionId);

                return Ok(new { message = "Transaction successfully softDeleted." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while soft deleting the transaction.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            } 
        }

        /// <summary>
        /// Get a summary of transactions, including total transactions,
        /// total credits, total debits, and net balance.
        /// </summary>
        /// <param name="filterDto">Filtering criteria (CustomerId, StartDate, EndDate).</param>
        /// <returns>A summary object with aggregated data.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("summary")]
        public async Task<IActionResult> GetTransactionSummary([FromQuery] CustomerFilterDto filterDto)
        {
            try
            {
                var summary = await transactionOrchestrationService.GetTransactionSummaryAsync(filterDto);

                if (summary == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorCode = ErrorCode.NotFound,
                        ErrorMessage = "No transactions found for the given criteria."
                    });
                }

                return Ok(summary);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching transaction summary.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}