using QrTailor.Application.Features.Auth.Constants;
using QrTailor.Application.Services.EmailService;
using QrTailor.Application.Services.Utils;
using QrTailor.Domain.Entities;
using QrTailor.Infrastructure.Results;
using QrTailor.Persistance.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace QrTailor.Application.Features.Auth.Queries
{
    public class GetEmailVerificationCodeUserQuery : IRequest<IRequestResult>
    {
        public int UserId { get; set; }
    }

    public class GetEmailVerificationCodeUserQueryHandler : IRequestHandler<GetEmailVerificationCodeUserQuery, IRequestResult>
    {
        private readonly DatabaseContext _context;
        private readonly IEmailService _emailService;
        private readonly IUtilsService _utilsService;

        public GetEmailVerificationCodeUserQueryHandler(DatabaseContext context, 
                                            IEmailService emailService,
                                            IUtilsService utilsService)
        {
            _context = context;
            _emailService = emailService;
            _utilsService = utilsService;
        }
        public async Task<IRequestResult> Handle(GetEmailVerificationCodeUserQuery request, CancellationToken cancellationToken)
        {
            var isUserExists = await _context.User.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

            if (isUserExists == null)
                return new ErrorRequestResult(AuthMessages.UserNotFound);

            isUserExists.RecoveryCode = _utilsService.GenerateRecoveryCode();

            _context.User.Update(isUserExists);
            int resultUpdate = _context.SaveChanges();

            if (resultUpdate == 0)
                return new ErrorRequestResult(AuthMessages.RecoveryCodeGenerationError);

            
            string emailBody = String.Format("Dear {0},<br>" +
                "This is your email verification code: <b>{1}</b><br>" +
                "Please enter this code into the field on your dashboard.<br>" +
                "Link-Map Team", isUserExists.FirstName, isUserExists.RecoveryCode);
            Email email = new Email
            {
                ToMail = isUserExists.Email,
                Body = emailBody,
                Subject = String.Format("[{0}] Link-Map password recovery code", ApplicationSettings.AppName)
            };

            var mailResult = _emailService.MailGonder(email);



            return new SuccessRequestResult(AuthMessages.RecoveryCodeGenerated);
        }
    }
}
