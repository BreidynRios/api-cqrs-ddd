using FluentValidation;

namespace Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .MaximumLength(50).WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

            RuleFor(e => e.Surname)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .MaximumLength(50).WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

            RuleFor(e => e.DocumentNumber)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .MaximumLength(20).WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

            RuleFor(e => e.Address)
                .MaximumLength(200).WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");
        }
    }
}
