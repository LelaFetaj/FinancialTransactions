using FinancialTransactionsAPI.Models.DTOs.Customers;
using FinancialTransactionsAPI.Models.Entities.Customers;
using FinancialTransactionsAPI.Models.Entities.Error;
using FinancialTransactionsAPI.Services.Foundations.Customers;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsAPI.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ILogger<CustomerController> logger;
        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateTransactionAsync([FromBody] CreateCustomerDto createCustomerDto)
        {
            try
            {
                var customerDto = await customerService.CreateCustomerAsync(createCustomerDto);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customerDto.CustomerId }, customerDto);
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
                logger.LogError(ex, "An error occurred during customer creation.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await customerService.RetrieveCustomerByIdAsync(id);

                if (customer == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorCode = ErrorCode.NotFound,
                        ErrorMessage = $"Customer with ID {id} not found."
                    });
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching the customer.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await customerService.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching customers.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ModifyCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            try
            {
                var customerDto = await customerService.ModifyCustomerAsync(updateCustomerDto);

                if (customerDto == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorCode = ErrorCode.NotFound,
                        ErrorMessage = "Customer not found."
                    });
                }

                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while modifying the customer.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> RemoveCustomerAsync(Guid customerId)
        {
            try
            {
                await customerService.RemoveCustomerAsync(customerId);
        
                return Ok(new { message = "Customer successfully deleted." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the customer.");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCode.InternalError,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}