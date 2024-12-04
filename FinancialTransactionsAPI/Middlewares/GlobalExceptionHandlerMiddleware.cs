using System.Net;
using FinancialTransactionsAPI.Models.Entities.Error;

namespace FinancialTransactionsAPI.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            // catch (ArgumentException ex)
            // {
            //     _logger.LogError(ex, "Argument exception occurred.");
            //     context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //     context.Response.ContentType = "application/json";
            //     var errorResponse = new ErrorResponse
            //     {
            //         ErrorCode = ErrorCode.BadRequest,
            //         ErrorMessage = ex.Message
            //     };
            //     await context.Response.WriteAsJsonAsync(errorResponse);
            // }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCode.TransactionCreationError,
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}