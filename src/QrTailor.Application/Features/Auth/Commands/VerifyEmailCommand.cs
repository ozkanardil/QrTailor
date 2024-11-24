using AutoMapper;
using MediatR;
using QrTailor.Application.Features.Auth.Constants;
using QrTailor.Infrastructure.Results;
using QrTailor.Persistance.Context;
using QrTailor.Infrastructure.Security.Hashing;
using QrTailor.Application.Services.EmailService;
using QrTailor.Application.Features.Auth.Models;

namespace QrTailor.Application.Features.Auth.Commands
{
    public class VerifyEmailCommand : VerifyEmailCommandRequest, IRequest<IRequestResult>
    {
        public int UserId { get; set; }
    }

    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, IRequestResult>
    {
        private readonly DatabaseContext _context;
        private readonly IEmailService _emailService;

        public VerifyEmailCommandHandler(IEmailService emailService,
                                        DatabaseContext context)
        {
            _emailService = emailService;
            _context = context;
        }
        public async Task<IRequestResult> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var userToCheck = _context.User.FirstOrDefault(u => u.Id == request.UserId && u.RecoveryCode == request.code);

            if (userToCheck == null)
                return new ErrorRequestResult(AuthMessages.InvalidCode);

            userToCheck.RecoveryCode = 0;
            userToCheck.Status = 1;

            _context.User.Update(userToCheck);
            int userCreateResult = _context.SaveChanges();

            if (userCreateResult == 0)
                return new ErrorRequestResult(AuthMessages.EmailVerificationFailed);

            return new SuccessRequestResult(AuthMessages.EmailVerified);
        }

    }


}
