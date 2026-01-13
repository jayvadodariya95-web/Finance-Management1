using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;

namespace FinanceManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountRepository _repository;
    private readonly ILogger<BankAccountController> _logger;

    public BankAccountController(IBankAccountRepository repository, ILogger<BankAccountController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BankAccountDto>>>> GetAll()
    {
        var accounts = await _repository.GetAllAsync();

        var dtos = accounts.Select(a => new BankAccountDto
        {
            Id = a.Id,
            BankName = a.BankName,
            AccountNumber = a.AccountNumber, // consider masking this later ****1234
            AccountHolderName = a.AccountHolderName,
            IfscCode = a.IfscCode,
            CurrentBalance = a.CurrentBalance,
            Currency = a.Currency,
            IsActive = a.IsActive
        });

        return Ok(ApiResponse<IEnumerable<BankAccountDto>>.SuccessResult(dtos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BankAccountDto>>> GetById(int id)
    {
        var a = await _repository.GetByIdAsync(id);
        if (a == null) return NotFound(ApiResponse<BankAccountDto>.ErrorResult("Account not found"));

        var dto = new BankAccountDto
        {
            Id = a.Id,
            BankName = a.BankName,
            AccountNumber = a.AccountNumber,
            AccountHolderName = a.AccountHolderName,
            IfscCode = a.IfscCode,
            CurrentBalance = a.CurrentBalance,
            Currency = a.Currency,
            IsActive = a.IsActive
        };

        return Ok(ApiResponse<BankAccountDto>.SuccessResult(dto));
    }

    [Authorize(Policy = "AdminOnly")] // Only Admin can add accounts
    [HttpPost]
    public async Task<ActionResult<ApiResponse<BankAccountDto>>> Create(CreateBankAccountDto request)
    {
        var account = new BankAccount
        {
            BankName = request.BankName,
            AccountNumber = request.AccountNumber,
            AccountHolderName = request.AccountHolderName,
            IfscCode = request.IfscCode,
            CurrentBalance = request.InitialBalance, // Initial Deposit
            Currency = request.Currency,
            IsActive = true
        };

        await _repository.CreateAsync(account);

        return Ok(ApiResponse<BankAccountDto>.SuccessResult(null, "Bank Account created successfully"));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        var success = await _repository.DeleteAsync(id);
        if (!success) return NotFound(ApiResponse<string>.ErrorResult("Account not found"));

        return Ok(ApiResponse<string>.SuccessResult(null, "Account deleted successfully"));
    }
}