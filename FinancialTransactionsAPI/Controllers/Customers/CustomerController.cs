using FinancialTransactionsAPI.Models.DTOs.Customers;
using FinancialTransactionsAPI.Services.Foundations.Customers;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsAPI.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateTransactionAsync([FromBody] CreateCustomerDto createCustomerDto)
        {
            if (createCustomerDto == null)
                return BadRequest("Customer data is required");

            var customerDto = await customerService.CreateCustomerAsync(createCustomerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customerDto.CustomerId }, customerDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id)
        {
            var customer = await customerService.RetrieveCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }
    }
}