using FinancialTransactionsAPI.Mappings.Customers;
using FinancialTransactionsAPI.Mappings.Transactions;
using FinancialTransactionsAPI.Middlewares;
using FinancialTransactionsAPI.Repositories.Customers;
using FinancialTransactionsAPI.Repositories.Transactions;
using FinancialTransactionsAPI.Services.Foundations.Customers;
using FinancialTransactionsAPI.Services.Foundations.Transactions;
using FinancialTransactionsAPI.Services.Orchestrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StorageBroker>();
builder.Services.AddScoped<StorageBroker>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ITransactionOrchestrationService, TransactionOrchestrationService>();
builder.Services.AddScoped<TransactionValidator>();
builder.Services.AddScoped<CustomerValidator>();

builder.Services.AddAutoMapper(typeof(TransactionMappingProfile), typeof(CustomerMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
