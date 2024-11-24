using FluentValidation;

namespace QrTailor.Application.Features.Auth.Queries
{
    public class GetRecoveryCodeQueryValidator : AbstractValidator<GetRecoveryCodeQuery>
    {
        public GetRecoveryCodeQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail address is required")
                .EmailAddress().WithMessage("Not a valid e-mail address");

        }
    }
}
