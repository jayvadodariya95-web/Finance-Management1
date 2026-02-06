using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceManagement.Application.Common;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Application.DTOs;
using System.Security.Claims;

namespace FinanceManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// Restrict access by role
[Authorize(Policy = "AdminOrPartner")]
public class FinancialController : ControllerBase
{
    private readonly IFinancialService _financialService;
    private readonly IBankTransactionRepository _transactionRepository;
    private readonly IPartnerRepository _partnerRepository;
    private readonly ILogger<FinancialController> _logger;

    public FinancialController(
        IFinancialService financialService,
        IBankTransactionRepository transactionRepository,
        IPartnerRepository partnerRepository,
        ILogger<FinancialController> logger)
    {
        _financialService = financialService;
        _transactionRepository = transactionRepository;
        _partnerRepository = partnerRepository;
        _logger = logger;
    }

    [Authorize(Policy = "AdminOrPartner")]
    [HttpGet("monthly-report/{month}/{year}")]
    public async Task<ActionResult<ApiResponse<MonthlyReportDto>>> GetMonthlyReport(int month, int year)
    {
        try
        {
            // BUG: No role authorization - any authenticated user can view financial reports
            // BUG: No validation for month/year parameters
            
            // if (month < 1 || month > 12)
            // {
            //     return BadRequest(ApiResponse<MonthlyReportDto>.ErrorResult("Invalid month"));
            // }
            if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12");

            if (year < 2000 || year > DateTime.UtcNow.Year)
            return BadRequest("Invalid year");
  
            var report = await _financialService.GenerateMonthlyReportAsync(month, year);
            
            return Ok(ApiResponse<MonthlyReportDto>.SuccessResult(report));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating monthly report for {Month}/{Year}", month, year);
            return StatusCode(500, ApiResponse<MonthlyReportDto>.ErrorResult("Failed to generate report"));
        }
    }

       [Authorize(Policy = "AdminOrPartner")]
       [HttpGet("partner-income/{partnerId}/{month}/{year}")]
        public async Task<ActionResult<ApiResponse<decimal>>> GetPartnerIncome(int partnerId, int month, int year)
        {
        //try
        //{
        //    // BUG: No authorization check - any user can view any partner's income
        //    // var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        //    // var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        //    var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        //    var partnerIdClaim = User.FindFirst("PartnerId")?.Value;


        //    // Prevent partners from accessing other partners' data
        //    if (userRole == "Partner")
        //    {
        //        if (string.IsNullOrEmpty(partnerIdClaim) || partnerIdClaim != partnerId.ToString())
        //            return Forbid("Partners can only access their own data");
        //    }

        try
        {
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // SECURITY CHECK: 
            // If the user is a Partner, strictly match the ID in the token.
            if (userRole == "Partner")
            {
                var tokenPartnerId = User.FindFirst("PartnerId")?.Value;

                // If token says ID is "1", but you asked for "99", this blocks you.
                if (string.IsNullOrEmpty(tokenPartnerId) || tokenPartnerId != partnerId.ToString())
                {
                    // Return 403 Forbidden with a clear message
                    return StatusCode(403, ApiResponse<decimal>.ErrorResult("Access Denied: You can only view your own data"));
                }
            }


            var partnerIncomes = await _financialService.CalculatePartnerIncomesAsync(month, year);
                var partnerIncome = partnerIncomes.FirstOrDefault(p => p.PartnerId == partnerId);

                if (partnerIncome == null)
                {
                    return NotFound(ApiResponse<decimal>.ErrorResult("Partner income not found"));
                }

                return Ok(ApiResponse<decimal>.SuccessResult(partnerIncome.ActualIncome));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving partner income");
                return StatusCode(500, ApiResponse<decimal>.ErrorResult("Failed to retrieve partner income"));
            }
        }

    [HttpPost("settlements/{month}/{year}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ApiResponse<string>>> ProcessSettlements(int month, int year)
    {
        try
        {
            // BUG: No role authorization - any authenticated user can process settlements
            // BUG: No validation to prevent duplicate processing
            
            await _financialService.ProcessSettlementsAsync(month, year);
            
            return Ok(ApiResponse<string>.SuccessResult("Settlements processed successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing settlements for {Month}/{Year}", month, year);
            return StatusCode(500, ApiResponse<string>.ErrorResult("Failed to process settlements"));
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("transactions")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BankTransactionDto>>>> GetTransactions(
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate)
    {
        try
        {
            IEnumerable<FinanceManagement.Domain.Entities.BankTransaction> transactions;
            
            if (startDate.HasValue && endDate.HasValue)
            {
                // PERFORMANCE ISSUE: No pagination for potentially large datasets
                transactions = await _transactionRepository.GetByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else
            {
                // BUG: Loading ALL transactions without any filtering
                transactions = await _transactionRepository.GetAllAsync();
            }

            var transactionDtos = transactions.Select(t => new BankTransactionDto
            {
                Id = t.Id,
                BankAccountName = t.BankAccount?.BankName ?? "Unknown",
                ProjectName = t.Project?.Name,
                Amount = t.Amount,
                Type = t.Type.ToString(),
                Description = t.Description,
                TransactionDate = t.TransactionDate,
                IsProcessed = t.IsProcessed
            });

            return Ok(ApiResponse<IEnumerable<BankTransactionDto>>.SuccessResult(transactionDtos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transactions");
            return StatusCode(500, ApiResponse<IEnumerable<BankTransactionDto>>.ErrorResult("Failed to retrieve transactions"));
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("transactions")]
    public async Task<ActionResult<ApiResponse<BankTransactionDto>>> CreateTransaction([FromBody] CreateTransactionDto request)
    {
        try
        {
            // BUG: No validation for required fields
            // BUG: No authorization check
            // BUG: No validation for reasonable amounts
            
            var transaction = new FinanceManagement.Domain.Entities.BankTransaction
            {
                BankAccountId = request.BankAccountId,
                ProjectId = request.ProjectId,
                Amount = request.Amount,
                Type = (FinanceManagement.Domain.Enums.TransactionType)request.Type,
                Description = request.Description,
                TransactionDate = request.TransactionDate,
                IsProcessed = false
            };

            var createdTransaction = await _transactionRepository.CreateAsync(transaction);
            
            var transactionDto = new BankTransactionDto
            {
                Id = createdTransaction.Id,
                Amount = createdTransaction.Amount,
                Type = createdTransaction.Type.ToString(),
                Description = createdTransaction.Description,
                TransactionDate = createdTransaction.TransactionDate,
                IsProcessed = createdTransaction.IsProcessed
            };

            return CreatedAtAction(nameof(GetTransactions), 
                ApiResponse<BankTransactionDto>.SuccessResult(transactionDto, "Transaction created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction");
            return StatusCode(500, ApiResponse<BankTransactionDto>.ErrorResult("Failed to create transaction"));
        }
    }

    [HttpGet("net-income/{month}/{year}")]
    public async Task<ActionResult<ApiResponse<decimal>>> GetNetIncome(int month, int year)
    {
        try
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");


            // BUG: Calculation includes bugs from FinancialService
            var netIncome = await _financialService.CalculateNetIncomeAsync(month, year);
            
            return Ok(ApiResponse<decimal>.SuccessResult(netIncome));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating net income");
            return StatusCode(500, ApiResponse<decimal>.ErrorResult("Failed to calculate net income"));
        }
    }
}

public class CreateTransactionDto
{
    public int BankAccountId { get; set; }
    public int? ProjectId { get; set; }
    public decimal Amount { get; set; }
    public int Type { get; set; } // TransactionType enum value
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
}

// CRITICAL ISSUES:
// 1. No role-based authorization for financial data
// 2. Any authenticated user can view sensitive financial information
// 3. No validation for financial amounts
// 4. No audit trail for financial operations
// 5. Missing pagination for large datasets
// 6. Calculation bugs inherited from service layer