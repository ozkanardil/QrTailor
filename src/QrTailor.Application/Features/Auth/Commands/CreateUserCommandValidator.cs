using FluentValidation;

namespace QrTailor.Application.Features.Auth.Commands
{
     public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.firstName).NotEmpty().WithMessage("Firt name must not be empty.");

            RuleFor(x => x.lastName).NotEmpty().WithMessage("Last name must not be empty.");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password must not be empty.")
                .Length(4, 8).WithMessage("Password must be at least 4 and maximum 8 characters.");

            RuleFor(x => x.email)
                .NotEmpty().WithMessage("E-mail must not be empty.")
                .EmailAddress().WithMessage("Enter a valid e-mail address.");
        }
    }
}
