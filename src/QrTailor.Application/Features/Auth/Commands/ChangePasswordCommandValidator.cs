using FluentValidation;

namespace QrTailor.Application.Features.Auth.Commands
{
     public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.code)
                .NotEmpty().WithMessage("Code must not be empty.")
                .GreaterThan(0).WithMessage("Code must be greater than zero")
                .InclusiveBetween(100000, 1000000).WithMessage("Code must be between 100.000 and 1.000.000");

            RuleFor(x => x.email)
                .NotEmpty().WithMessage("E-mail must not be empty.")
                .EmailAddress().WithMessage("It is not a valid e-mail address.");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password must not be empty.")
                .Length(4, 8).WithMessage("Password must be at least 4 and maximum 8 characters.");

            RuleFor(x => x.passwordConfirmation)
                .NotEmpty().WithMessage("Password confirmation must not be empty.")
                .Length(4, 8).WithMessage("Password confirmation must be at least 4 and maximum 8 characters.")
                .Equal(x => x.password).WithMessage("Password and password confirmation must be the same.");
        }
    }
}
