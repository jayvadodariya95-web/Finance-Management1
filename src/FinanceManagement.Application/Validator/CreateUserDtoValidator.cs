using FluentValidation;
using FinanceManagement.Application.DTOs;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            //.EmailAddress()
            .MaximumLength(100);

        //RuleFor(x => x.Password)
        //    .NotEmpty()
        //    .MinimumLength(8)
        //    .MaximumLength(64);

        //RuleFor(x => x.Role)
        //    .GreaterThan(0);
    }
}

public class CreatePartnerDtoValidator : AbstractValidator<PartnerDto>
{
    public CreatePartnerDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.SharePercentage)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}

public class CreateProjectDtoValidator : AbstractValidator<ProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ClientName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ProjectValue)
            .GreaterThan(0)
            .LessThan(1_000_000_000);

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue);

        RuleFor(p => p.ProjectValue)
           .GreaterThanOrEqualTo(0)
           .WithMessage("Project value must be positive");
    }
}

public class CreateExpenseDtoValidator : AbstractValidator<ExpenseDto>
{
    public CreateExpenseDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .LessThan(100_000_000);

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}

public class CreateBankTransactionDtoValidator : AbstractValidator<BankTransactionDto>
{
    public CreateBankTransactionDtoValidator()
    {
        RuleFor(x => x.Amount)
            .NotEqual(0)
            .LessThan(1_000_000_000);

        RuleFor(x => x.Type)
            .NotEmpty();

        RuleFor(x => x.TransactionDate)
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}

