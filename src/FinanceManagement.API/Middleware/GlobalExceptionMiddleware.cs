using System.Net;
using System.Text.Json;
using FinanceManagement.Application.Common;

namespace FinanceManagement.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred processing request");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new ApiResponse<object>();

        switch (exception)
        {
            case ArgumentNullException:
                response.Success = false;
                response.Message = "Invalid request data";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            
            case UnauthorizedAccessException:
                response.Success = false;
                response.Message = "Unauthorized access";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            
            case KeyNotFoundException:
                response.Success = false;
                response.Message = "Resource not found";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            
            default:
                response.Success = false;
                response.Message = "An internal server error occurred";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}