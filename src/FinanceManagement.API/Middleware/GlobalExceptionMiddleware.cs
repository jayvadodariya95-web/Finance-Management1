using System.Net;
using System.Text.Json;
using FinanceManagement.Application.Common;

namespace FinanceManagement.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env; // ✅ Added Environment Support

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // ✅ 1. Generate Trace ID
            var traceId = Guid.NewGuid().ToString();

            // ✅ 2. SECURITY: Sanitize the message for the Server Logs
            // (Your helper handles the User Response, this handles the Server Log)
            var safeMessage = LogSanitizer.Sanitize(ex.Message);

            _logger.LogError(ex,
                "ErrorId: {TraceId} | Message: {SafeMessage}",
                traceId,
                safeMessage);

            // ✅ 3. Call Your Excellent Helper Method
            await HandleExceptionAsync(context, ex, traceId, _env.IsDevelopment());
        }
    }

    // ⭐ THIS IS YOUR OPTIMIZED METHOD (Static & Parameterized)
    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        string traceId,
        bool isDevelopment)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiResponse<object>
        {
            Success = false
        };

        switch (exception)
        {
            case ArgumentNullException:
            case ArgumentException:
                response.Message = "Invalid request data";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case UnauthorizedAccessException:
                response.Message = "Unauthorized access";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case KeyNotFoundException:
                response.Message = "Resource not found";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            default:
                // Smart Logic: Show real error in Dev, Generic in Prod
                response.Message = isDevelopment
                    ? exception.Message
                    : "An unexpected error occurred. Please contact support.";

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        // Smart Logic: Show stack trace in Dev, only TraceID in Prod
        if (isDevelopment)
        {
            response.Data = new
            {
                TraceId = traceId,
                Exception = exception.GetType().Name,
                StackTrace = exception.StackTrace
            };
        }
        else
        {
            response.Data = new
            {
                TraceId = traceId
            };
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}