using FluentValidation;

namespace QrTailor.Application.Features.Auth.Commands
{
     public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(x => x.code).GreaterThan(0).WithMessage("Code must be greater than zero.");
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than zero.");

        }
    }
}
