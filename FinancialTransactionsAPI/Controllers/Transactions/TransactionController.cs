using FinancialTransactionsAPI.Models.DTOs.Transactions;
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

        public TransactionController(ITransactionOrchestrationService transactionOrchestrationService)
        {
            this.transactionOrchestrationService = transactionOrchestrationService;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransactionAsync([FromBody] CreateTransactionDto createTransactionDto)
        {
            if (createTransactionDto == null)
                return BadRequest("Transaction data is required");

            var result = await transactionOrchestrationService.CreateTransactionAsync(createTransactionDto);

            if (result.TransactionId == Guid.Empty)
            {
                return BadRequest("Transaction creation failed. No ID was generated.");
            }

            return CreatedAtAction(nameof(RetrieveTransactionByIdAsync), new { id = result.TransactionId }, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetFilteredTransactions([FromQuery] TransactionFilter filter)
        {
            var transactions = await transactionOrchestrationService.GetFilteredTransactionsAsync(filter);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> RetrieveTransactionByIdAsync(Guid id)
        {
            var transaction = await transactionOrchestrationService.GetTransactionByIdAsync(id);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpPut]
        public async Task<IActionResult> ModifyTransactionAsync(UpdateTransactionDto updateTransactionDto)
        {
            if (updateTransactionDto == null)
                return BadRequest("Transaction data is required.");

            var transactionDto = await transactionOrchestrationService.ModifyTransactionAsync(updateTransactionDto);

            if (transactionDto == null)
                return NotFound("Transaction not found.");

            return Ok(transactionDto);
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> RemoveTransactionAsync(Guid transactionId)
        {
            await transactionOrchestrationService.RemoveTransactionAsync(transactionId);
            return NoContent(); 
        }

        [HttpDelete("softDeletion")]
        public async Task<IActionResult> SoftDeletionTransactionAsync(Guid transactionId)
        {
            await transactionOrchestrationService.SoftDeleteTransactionAsync(transactionId);
            return NoContent(); 
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
            var summary = await transactionOrchestrationService.GetTransactionSummaryAsync(filterDto);

            if (summary == null)
                return NotFound("No transactions found for the given criteria.");

            return Ok(summary);
        }

    }
}